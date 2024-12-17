using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Configuration;
using System.Reflection;
using System.Collections;
using RSC_Models;
using RSC_Configurations;
using RSC_FileProcessor;

namespace RSC
{
    public class JobCreation 
    {
        static void Main(string[] args)
        {

            string[] pathfolderStoreCodes = Directory.GetDirectories(AppConfiguration.mainFolderPath);//get all paths on my local folders

            List<StoreModel> DBStorecodes = GetStorecodesFromDB();///get all storecodes by using DB

            foreach (string folderstorecodes in pathfolderStoreCodes)
            {
                int storeId = default;

                string storeDirName = Path.GetFileName(folderstorecodes);

                bool isvalid = false;
                foreach (var x in DBStorecodes)
                {
                    if (x.StoreCode == storeDirName)
                    {
                        storeId = x.StoreId;
                        isvalid = true;
                        break;
                    }
                }
                if (!isvalid)
                {
                    continue;
                }
                string[] storefolderfiles = Directory.GetFiles(folderstorecodes.ToString());
                string storeFilePath = string.Empty;
                string stockFilePath = string.Empty;
                string employeeFilePath = string.Empty;
                string customerFilePath = string.Empty;
                foreach (string file in storefolderfiles)
                {
                    if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("stores_"))
                    {
                        storeFilePath = file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("stock_"))
                    {
                        stockFilePath = file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("employee_"))
                    {
                        employeeFilePath = file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("customer"))
                    {
                        customerFilePath = file;
                    }
                }
                //////////////////
                ///StoreProcessor
                ////////////////

                new StockProcessor(storeFilePath, storeId, storeDirName).Processor();

                //////////////////
                ///stockFile process
                //////////////////

                new StockProcessor(storeFilePath, storeId, storeDirName).Processor();

                ////////////////
                ///employee processing
                ////////////////

                new EmployeProcesser(employeeFilePath, storeId, storeDirName).processor();

                /////////////////
                ///Customer processing
                ///////////

                new CustumerProcessor(customerFilePath, storeId).processor();

                //////////////////
                ///stockFile process
                //////////////////
            }
        }
        private static List<StoreModel> GetStorecodesFromDB()
        {
            List<StoreModel> storescodes = new List<StoreModel>();
            using (SqlConnection conn = new SqlConnection(AppConfiguration.dbConnectionstring))
            {
                using (SqlCommand cmd = new SqlCommand("select storeIDPk,storeCode,StoreName,Location,ManagerName,ContactNumber from stores", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            StoreModel model = new StoreModel();

                            model.StoreId = Convert.ToInt32(reader["storeIDPk"]);
                            model.StoreCode = Convert.ToString(reader["storeCode"]);
                            model.StoreName = Convert.ToString(reader["StoreName"]);
                            model.Location = Convert.ToString(reader["Location"]);
                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
                            model.ContactNumber = Convert.ToString(reader["ContactNumber"]);
                            storescodes.Add(model);
                        }
                    }
                }
                conn.Close();
            }
            return storescodes;
        }
    }
}
