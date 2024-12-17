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
    internal class BatchJob
    {
        static string rootFolderPath { get; set; }
        static string rSCConnectionString { get; set; }

        static BatchJob()
        {
            rootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            rSCConnectionString = ConfigurationManager.ConnectionStrings["iConnectRSCConnectionString"].ToString();
        }

        static void Main(string[] args)
        {
            //Step-1 : Source Information (Shared Location)
            //Destination Information ( SQL SERVER DATABASE)

            //Step-2:
            //Read the Shared location and get all the files.
           

            string[] storeDirectories = Directory.GetDirectories(rootFolderPath);
            //Option1 : to get folder name using Path Class (Recommended)
            //string storeDirName = Path.GetFileName(storeDirectories[0]);
            //Option 2: using array (Brute force)
            //string storeDirName1 = storeDirectories[0].Split('\\')[storeDirectories[0].Split('\\').Length - 1];
            //\\MURALI\iConnect\RSC\DailyDataLoad\STBLR003


                  List<StoreModel> stores = GetAllStoresFromDB();

            foreach (string storeDirectoryPath in storeDirectories)
            {
                string storeDirName = Path.GetFileName(storeDirectoryPath);

                //Every time a new store folder will come.
                //bool isValidFolder = ValdateStoreFolderInDB(storeDirName);//Not required=> we no need to hit db for every folder validation.
                bool isValidFolder = stores.Exists(x => x.StoreCode == storeDirName);//Lambda (=>)

                //by using loops verifying the folder
                bool isValid = false;
                foreach (var x in stores)
                {
                    if (x.StoreCode == storeDirName)
                    {
                        isValid = true;
                        break;
                    }
                }

                //if (!isValidFolder)
                if (!stores.Exists(x => x.StoreCode == storeDirName))
                {
                    //Ignore the folder as it doesn't match with DB Stores.
                    continue;
                }

                //Store
                //Stock
                //Employee
                string[] storeFolderFiles = Directory.GetFiles(storeDirectoryPath);
                //Get all file names
                string storeFilePath = string.Empty;
                string stockFilePath = string.Empty;
                string employeeFilePath = string.Empty;
                foreach (string file in storeFolderFiles)
                {
                    if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("store_"))
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
                     
                }

                //////////////////////////////////////////////
                /////Store file Processing
                //////////////////////////////////////////////
                //step-1 read data from file.
                //step-2 validate the data
                //step-3 update/insert/push the data to DB;
                DataTable dataTable = new DataTable();
                string filecontent = File.ReadAllText(storeFilePath);//free text content
                using (OleDbConnection con = new OleDbConnection())//option -2 (atleast header line mandatory in the file)
                {
                    con.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + storeDirectoryPath + ";Extended Properties='text;HDR=Yes;FMT=Delimited(,)'";
                    using (OleDbDataAdapter da = new OleDbDataAdapter("select * from " + Path.GetFileName(storeFilePath), con))
                    {
                        da.Fill(dataTable);
                    }
                }
                string[] filecontentLines = File.ReadAllLines(storeFilePath);//option-1
                //validate if file has no content
                if (filecontentLines.Length < 1)
                {
                    Console.WriteLine("Log the error: Invalid file");
                    continue;
                }
                //validate if file has only header row
                else if (filecontentLines.Length == 1)
                {
                    Console.WriteLine("Log the warning: No data present in the file");
                    continue;
                }
                else if (filecontentLines.Length > 2)
                {
                    Console.WriteLine("Log the error: Invalid file; has multiple store records.");
                    continue;
                }

                string[] data = filecontentLines[1].Split(',');

                if(data.Length != 6)
                {
                    Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 6.");
                    continue;
               

                StoreModel model = new StoreModel();
                model.StoreCode = data[1];
                model.StoreName = data[2];
                model.Location = data[3];
                model.ManagerName = data[4];
                model.ContactNumber = data[5];

                if(model.StoreCode.ToLower() != storeDirName.ToLower())
                {
                    Console.WriteLine("Log the error: Invalid data; store code not matching with current storecode.");
               
                        continue;
                }
                int rowsAffected = SyncStoreTableData(model);

                if(rowsAffected > 0)
                {
                    Console.WriteLine("Log the Information: Storefile sync with DB is sucess.");
                }
                else
                {
                    Console.WriteLine("Log the Warning: Storefile sync with DB is not sucess.");
                }

                string sourcePath = storeFilePath;
                //Store folderpath + Processed + Store_yyymmmdd.csv
                string destPath = Path.Combine(storeDirectoryPath, "Processed", Path.GetFileName(storeFilePath));
                File.Move(sourcePath, destPath);

                //////////////////////////////////////////////
                /////Stock file Processing
                //////////////////////////////////////////////


                //////////////////////////////////////////////
                /////Employee file Processing
                //////////////////////////////////////////////



                ///files reading
            }

            Console.Read();

            //Step-3:
            //Read the content of the file
            //Step-4:
            //Validation : Validate the data against to the business rules.

            //Step-5:
            //Push the data to the DB.
        }

        private static int SyncStoreTableData(StoreModel model)
        {
            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                string query = $"Update Stores Set StoreName = @StoreName, Location = @Location, ManagerName= @Manager, ContactNumber = @ContactNumber where StoreCode = @StoreCode";

                string query1 = $"Update Stores Set StoreName = {model.StoreName}, Location = {model.Location}, ManagerName= {model.ManagerName}, ContactNumber = {model.ContactNumber} where StoreCode = {model.StoreCode}";

                con.Open();
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@StoreName",DbType.String).Value = model.StoreName;
                    cmd.Parameters.Add("@StoreCode", DbType.String).Value = model.StoreCode;
                    cmd.Parameters.Add("@Location", DbType.String).Value = model.Location;
                    cmd.Parameters.Add("@Manager", DbType.String).Value = model.ManagerName;
                    cmd.Parameters.Add("@ContactNumber", DbType.String).Value = model.ContactNumber;
                    int rowsaffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsaffected;
                }
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
                            StoreModel model = new StoreModel();
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

        private static bool ValdateStoreFolderInDB(string storeDirName)
        {
            //Option 3 : using DataAdapter and Dataset/datatable
            {
                using (SqlConnection con1 = new SqlConnection(rSCConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("select * from Stores where storecode = '" + storeDirName + "';select * from Stores;select * from Stores;select * from Stores;select * from Stores;select * from Stores;select * from Stores;", con1))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt.Rows.Count > 0;
                        }
                    }
                }





               
            }

            //Option 2 : DataReader
            {
                int storeId = default(int);
                using (SqlConnection con = new SqlConnection(rSCConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "select * from Stores where storecode = '" + storeDirName + "'";
                        //cmd.CommandType = System.Data.CommandType.Text;//this is by default so we no need write it explicitly
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //storeId = (int)reader[0];
                                int storeId1 = (int)reader["StoreIdpk"];
                                //StoreCode StoreName   Location ManagerName ContactNumber
                                string StoreCode = reader["StoreCode"].ToString();
                                string Storename = Convert.ToString(reader["StoreName"]);
                                string Location = Convert.ToString(reader["Location"]);
                                string ManagerName = Convert.ToString(reader["ManagerName"]);
                                string ContactNumber = Convert.ToString(reader["ContactNumber"]);
                            }
                        }
                        con.Close();
                    }
                }

                //return storeId > 0;
            }
            //Option 1;
            {
                int StoreId;
                using (SqlConnection con = new SqlConnection(rSCConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "select StoreIdPk from Stores where storecode = '" + storeDirName + "'";
                        //cmd.CommandType = System.Data.CommandType.Text;//this is by default so we no need write it explicitly
                        cmd.Connection = con;
                        con.Open();
                        StoreId = Convert.ToInt32(cmd.ExecuteScalar());
                        //int StoreId1 = (int)(cmd.ExecuteScalar());
                        //int StoreId11 = int.Parse(cmd.ExecuteScalar().ToString());
                    }
                    con.Close();
                }
                return StoreId > 0;
            }
        }
    }

}
