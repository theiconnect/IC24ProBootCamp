
using Kiran_RSC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.AppConnection_Kiran;
using RSC.FileModel_Kiran;
using BusinessAccessLayer;
using RSC_IDAL;
using RSCEntityDSAL;
using DataBaseAccessLayer;


namespace Kiran_RSC
{
    internal class BatchJob : AppConnection
    {
        private static IStoreDAL StoreDALObj {  get; set; }    
        private static IEmployeeDAL EmployeeDALObj { get; set; }
        private static IStockDAL StockDALobj { get; set; }

        static BatchJob()
        {
            if (UseEF)
            {
                StoreDALObj = new StoreEntityDAL(); /// by using  entity framework class
            }
            else
            {
                StoreDALObj = new SyncStoreDataToDB(); /// by using ADO.net class
            }
        }
        static void Main(string[] args)
        {
            string[] storeDirectories = Directory.GetDirectories(rootFolderPath);

            List<StoreModel> stores = GetAllStoresFromDB();
            int storeid = default;
            foreach (string storeDirectoryPath in storeDirectories)
            {
                string storeDirName = Path.GetFileName(storeDirectoryPath);

                //Every time a new store folder will come.
                //bool isValidFolder = ValdateStoreFolderInDB(storeDirName);//Not required=> we no need to hit db for every folder validation.
                //bool isValidFolder = stores.Exists(x => x.storeCode == storeDirName);//Lambda (=>)

                bool isvalid = false;
                foreach (var x in stores)
                {
                    x.storeCode = storeDirName;
                    storeid = x.storeId;
                    isvalid = true;
                    break;
                }
                if (!isvalid)
                    if (!stores.Exists(x => x.storeCode == storeDirName))
                    {
                        continue;
                    }

                
                string[] storeFolderFiles = Directory.GetFiles(storeDirectoryPath);
                //Get all file names
                string storeFilePath = string.Empty;
                string stockFilePath = string.Empty;
                string employeeFilePath = string.Empty;
                string customerFilePath = string.Empty;
                foreach (string file in storeFolderFiles)
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
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("customer_"))
                    {
                        customerFilePath = file;
                    }

                }
                //////////////////////////////////////////////
                /////Store file Processing
                //////////////////////////////////////////////


                new StoreProcesser(storeFilePath, StoreDALObj).Processer();



                ////////////////////////////////////////////////
                /////Stock File Processing
                ///////////////////////////////////////////////

                new StockProcesser(stockFilePath, storeid, storeDirName,StockDALobj).Processor();



                ////////////////////////////////////////////
                /////Employee file processing
                ///////////////////////////////////////////
                //new EmployeeProcesser(employeeFilePath).Processer();

                new Employeeprocesser(employeeFilePath, storeid, storeDirName, EmployeeDALObj).processer();



            }

        }

        private static List<StoreModel> GetAllStoresFromDB()
        {
            List<StoreModel> stores = new List<StoreModel>();
            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT StoreIdPk,StoreCode,StoreName,Location,ManagerName,ContactNumber FROM Stores", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StoreModel model1 = new StoreModel();
                            model1.storeId = Convert.ToInt32(reader["StoreIdPk"]);
                            model1.storeCode = Convert.ToString(reader["StoreCode"]);
                            model1.storeName = Convert.ToString(reader["Storename"]);
                            model1.location = Convert.ToString(reader["Location"]);
                            model1.managerName = Convert.ToString(reader["ManagerName"]);
                            model1.contactNumber = Convert.ToString(reader["ContactNumber"]);
                            stores.Add(model1);
                        }
                    }
                }
                con.Close();
            }
            return stores;  
        }
    }
}
