
using Kiran_RSC;
using Kiran_RSC.MODELS;
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


namespace Kiran_RSC
{
    internal class BatchJob : AppConnection
    {

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
                foreach(var x in stores)
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


                new StoreProcesser(storeFilePath).Processer();

                ////////////////////////////////////////////
                /////Employee file processing
                ///////////////////////////////////////////
                 //new EmployeeProcesser(employeeFilePath).Processer();

                new Employeeprocesser(employeeFilePath, storeid, storeDirName).processer();
                

                    //step-1 read data from file.
                    //step-2 validate the data
                    //step-3 update/insert/push the data to DB;

                    //    string[] filecontentLines = File.ReadAllLines(storeFilePath); /*[if define the string array]*/
                    //    //validate if file has no content
                    //    if (filecontentLines.Length < 1)
                    //    {
                    //        Console.WriteLine("Log the error: Invalid file");
                    //        continue;
                    //    }
                    //    //validate if file has only header row
                    //    else if (filecontentLines.Length == 1)
                    //    {
                    //        Console.WriteLine("Log the warning: No data present in the file");
                    //        continue;
                    //    }
                    //    else if (filecontentLines.Length > 2)
                    //    {
                    //        Console.WriteLine("Log the error: Invalid file; has multiple store records.");
                    //        continue;
                    //    }

                    //    string[] data = filecontentLines[1].Split(',');

                    //    if (data.Length != 6)
                    //    {
                    //        Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 6.");
                    //        continue;
                    //    }

                    //    StoreModel model = new StoreModel();
                    //    model.StoreCode = data[1];
                    //    model.StoreName = data[2];
                    //    model.Location = data[3];
                    //    model.ManagerName = data[4];
                    //    model.ContactNumber = data[5];

                    //    if (model.StoreCode.ToLower() != storeDirName.ToLower())
                    //    {
                    //        Console.WriteLine("Log the error: Invalid data; store code not matching with current storecode.");
                    //        continue;
                    //    }
                    //    int rowsAffected = SyncStoreTableData(model);

                    //    if (rowsAffected > 0)
                    //    {
                    //        Console.WriteLine("Log the Information: Storefile sync with DB is sucess.");
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Log the Warning: Storefile sync with DB is not sucess.");
                    //    }

                    //    string sourcePath = storeFilePath;
                    //    //Store folderpath + Processed + Store_yyymmmdd.csv
                    //    string destPath = Path.Combine(storeDirectoryPath, "Processed", Path.GetFileName(storeFilePath));
                    //    File.Move(sourcePath, destPath);

                    //    //SyncStoreTableData(model);
                    //}

                    //Step-3:
                    //Read the content of the file

                    //Step-4:
                    //Validation : Validate the data against to the business rules.

                    //Step-5:
                    //Push the data to the DB.

                }
            //private static int SyncStoreTableData(StoreModel model)
            //{
            //    using (SqlConnection con = new SqlConnection(rSCConnectionString))
            //    {
            //        string query = $"Update Stores Set StoreName = @StoreName, Location = @Location, ManagerName= @Manager, ContactNumber = @ContactNumber where StoreCode = @StoreCode";

            //        string query1 = $"Update Stores Set StoreName = {model.StoreName}, Location = {model.Location}, ManagerName= {model.ManagerName}, ContactNumber = {model.ContactNumber} where StoreCode = {model.StoreCode}";

            //        con.Open();
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandText = query;
            //            cmd.Connection = con;
            //            cmd.Parameters.Add("@StoreName", DbType.String).Value = model.StoreName;
            //            cmd.Parameters.Add("@StoreCode", DbType.String).Value = model.storeCode;
            //            cmd.Parameters.Add("@Location", DbType.String).Value = model.Location;
            //            cmd.Parameters.Add("@Manager", DbType.String).Value = model.ManagerName;
            //            cmd.Parameters.Add("@ContactNumber", DbType.String).Value = model.ContactNumber;
            //            int rowsaffected = cmd.ExecuteNonQuery();
            //            con.Close();
            //            return rowsaffected;
            //        }
            //    }
            //}




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
