using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC
{
    internal class ProgramV2
    {
        static int storeIdPk { get; set; }
        public static string stockFilePath { get; private set; }
        public static string mainFolderPath { get; private set; }
        public static string rscConnectedString { get; private set; }

        static void Main(string[] args)

        {
            //Get all the store folders from root directory
            string[] directories = Directory.GetDirectories(mainFolderPath);
            //Get All Stores information From DB
            List<StoreModel> storesData = GetAllStoresDataFromDB();

            foreach (string storeDirectoryPath in directories)
            {
                //Get Store Directory Path
                string storeDirName = Path.GetFileName(storeDirectoryPath);

                var store = storesData.Exists(x => x.StoreCode == storeDirName);

                if (!store)
                {
                    //there is no store found with the storecode matching with foldername leave that and go and check next storecode folder
                    continue;
                }
                //Get the storefile path from the directory
                string storeFilePath = GetFileNameByFileType(storeDirectoryPath, FileTypes.Stores); //"Stores"
                //Initiate store file processing by using store processor
                var storeProcessor = new StoreProcessor(storeFilePath);
                storeProcessor.Process();

                string StockFilePath = GetFileNameByFileType(storeDirectoryPath, FileTypes.Stock);
                var stockProcessor = new StockProcess(StockFilePath, storeIdPk);
                stockProcessor.Process();


                string EmployeeFilePath = GetFileNameByFileType(storeDirectoryPath, FileTypes.Employee);
                var employeeProcessor = new EmployeeProcessor(EmployeeFilePath);
                employeeProcessor.Process();

                string CustomerFilePath = GetFileNameByFileType(storeDirectoryPath, FileTypes.Customer);
                var customerProcessor = new CustomerProcess(CustomerFilePath, storeIdPk);
                customerProcessor.Process();


            }





            Console.Read();





        }
        private static string GetFileNameByFileType(string storeDirectoryPath, FileTypes fileType)
        {
            //get all files from store directory
            string[] storeLevelFiles = Directory.GetFiles(storeDirectoryPath);
            string startWithValue = string.Empty;
            switch (fileType)
            {
                case FileTypes.Stores:
                    startWithValue = "stores_"; break;

                case FileTypes.Stock:
                    startWithValue = "stock_"; break;

                case FileTypes.Employee:
                    startWithValue = "employee_"; break;

                case FileTypes.Customer:
                    startWithValue = "customer_"; break;


            }
            foreach (string file in storeLevelFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith(startWithValue))
                {
                    return file;
                }
            }
            return null;
        }


        private static List<StoreModel> GetAllStoresDataFromDB()
        {
            List<StoreModel> stores = new List<StoreModel>();
            using (SqlConnection con = new SqlConnection(rscConnectedString))
            {
                //string query = "select StoreIdPk,storeCode,StoreName,Location,ManagerName,StoreContactNumber from stores";
                using (SqlCommand command = new SqlCommand())
                {
                    con.Open();
                    command.CommandText = "GetAllStores";
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StoreModel model = new StoreModel();
                            model.StoreIdPk = Convert.ToInt32(reader["StoreIdPk"]);
                            model.StoreCode = Convert.ToString(reader["storeCode"]);
                            model.StoreName = Convert.ToString(reader["StoreName"]);
                            model.Location = Convert.ToString(reader["Location"]);
                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
                            model.ContactNumber = Convert.ToString(reader["StoreContactNumber"]);
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
