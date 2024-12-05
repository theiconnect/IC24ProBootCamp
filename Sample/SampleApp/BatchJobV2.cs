using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using SampleApp.Models;
using System.Threading;
using System.Data.OleDb;

namespace SampleApp
{
    internal class BatchJobV2
    {
        static string rootFolderPath { get; set; }
        static string rSCConnectionString { get; set; }

        static BatchJobV2()
        {
            rootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            rSCConnectionString = ConfigurationManager.ConnectionStrings["iConnectRSCConnectionString"].ToString();
        }

        static void Main(string[] args)
        {
            //Get all the store folders from root directory
            string[] storeDirectories = Directory.GetDirectories(rootFolderPath);

            //Get all the stores information from DB
            List<StoreModel> stores = GetAllStoresFromDB();

            foreach (string storeDirectoryPath in storeDirectories)
            {
                string storeDirName = Path.GetFileName(storeDirectoryPath);
                var store = stores.Find(x => x.StoreCode == storeDirName);
                if(store == null)
                {
                    //there is no store found with the storecode matching with foldername
                    continue;
                }

                //Get the storefile path from the directory
                string storeFilePath = GetStoreFileNames(storeDirectoryPath, FileTypes.Store);
                //Initiate store file processing by using store processor
                new StoreProcessor(storeFilePath).Process();
            }
            Console.Read();
        }

        private static string GetStoreFileNames(string storeDirectoryPath, FileTypes fileType)
        {
            string[] storeFolderFiles = Directory.GetFiles(storeDirectoryPath);
            string startwithvalue = string.Empty;
            switch(fileType)
            {
                case FileTypes.Store:
                    startwithvalue = "store_";break;

                case FileTypes.Stock:
                    startwithvalue = "stock_"; break;

                case FileTypes.Employee:
                    startwithvalue = "employee_"; break;

            }

            foreach (string file in storeFolderFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith(startwithvalue))
                {
                    return file;
                }               
            }
            return null;
        }

        private static List<StoreModel> GetAllStoresFromDB()
        {
            var stores = new List<StoreModel>();
            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT StoreIdPk,StoreCode,StoreName,Location,ManagerName,ContactNumber FROM Stores", con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var model = new StoreModel();
                            model.StoreId = Convert.ToInt32(reader["StoreIdPk"]);
                            model.StoreCode = Convert.ToString(reader["StoreCode"]);
                            model.StoreName = Convert.ToString(reader["Storename"]);
                            model.Location = Convert.ToString(reader["Location"]);
                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
                            model.ContactNumber = Convert.ToString(reader["ContactNumber"]);
                            stores.Add(model);
                        }
                    }
                }
                con.Close();
            }
            return stores;
        }
    }
}
