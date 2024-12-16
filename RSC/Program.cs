//using System;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using System.Data.SqlClient;
//using System.ComponentModel.Design;
//using System.Configuration;
//using System.Security.Cryptography;
//using Rsc_Task.StoreModels;
//using System.Reflection;
//using System.Windows.Input;
//using System.Runtime.Remoting.Messaging;


//namespace RSC
//{
//    internal class Program
//    {

//        static string mainFolderPath { get; set; }
//        static string rscConnectedString { get; set; }

//        static Program()
//        {
//            mainFolderPath = ConfigurationManager.AppSettings["RootFolder"];
//            rscConnectedString = ConfigurationManager.ConnectionStrings["iConnectRSCConnectionString"].ToString();


//        }
//        static void Main(string[] args)

//        {
//            string[] directories = Directory.GetDirectories(mainFolderPath);
//            List<StoreModel> storesData = GetAllStoresDataFromDB();
//           // List<StockBO> Products = GetrAllProductsFromDB();
//            List<EmployeeDTO> employeeData = GetAllEmployeeDataFromDB();



//            foreach (string storeDirectoryPath in directories)
//            {
//                string storeDirName = Path.GetFileName(storeDirectoryPath);
//                Console.WriteLine(storeDirName);

//                bool isValidFolderData = ValdateStoreFolderInDB(storeDirName);
//                bool isValidFolder = storesData.Exists(x => x.StoreCode == storeDirName);
//                //the above one we are using lambda function
//                //here we are match the storeCode in storetable and storeCode in DailyDataLoad folder 
//                //x means storetable=>store.storecode
//                if(!storesData.Exists(x => x.StoreCode == storeDirName))
//                {
//                    continue;
//                    //condition true means continue check next iteration
//                    //condition false means not go to loop
//                }
//                string[] storeFolderFiles = Directory.GetFiles(storeDirectoryPath);
//                string storeFilePath=string.Empty;
//                string employeeFilePath = string.Empty;
//                string stockFilePath=string.Empty;
//                foreach (string file in storeFolderFiles ) 
//                {
//                    if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("stores_")) 
//                    { 
//                        storeFilePath = file;
//                    }
//                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("employee_"))
//                    {
//                        employeeFilePath = file;
//                    }
//                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("stock_"))
//                    {
//                        stockFilePath = file;

//                    }
//                }


//                //-----------------------------------------------------
//                /////Store file Processing
//                //----------------------------------------------------
//                //step-1 read data from file.
//                //step-2 validate the data
//                //step-3 update/insert/push the data to DB;

//                //read store file data
//                string[] fileContentLines=File.ReadAllLines(storeFilePath);
//                //validate file data Content
//                if (fileContentLines.Length < 1)
//                {
//                    Console.WriteLine("Log the error: Invalid file");
//                    continue;

//                }
//                else if (fileContentLines.Length > 2)
//                {
//                    Console.WriteLine("Log the error: Invalid file; has multiple store records.");
//                    continue;
//                }
//                else if (fileContentLines.Length == 1)
//                {
//                    Console.WriteLine("Log the warning: No data present in the file");
//                    continue;

//                }
//                string[] fileData = fileContentLines[1].Split(',');
//                //fileContentLines[0] contains headings

//                if (fileData.Length != 6)
//                { 
//                    //after split the data is coming in array .in that all 6 elemnts are tbeir because in my store table 6 columns.6 elements not coming means error.
//                    Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 6 fileds data.");
//                    continue;

//                }
//                //we are creating two object for models
//                //one for storing database data and another one for storing files data
//                StoreModel storeModelObject = new StoreModel();
//                //storeId is not taking because it is auto incremented file
//                storeModelObject.StoreCode=fileData[1];
//                storeModelObject.StoreName = fileData[2];
//                storeModelObject.Location=fileData[3];
//                storeModelObject.ManagerName=fileData[4];
//                storeModelObject.ContactNumber = fileData[5];

//                if (storeModelObject.StoreCode.ToLower() != storeDirName.ToLower())
//                {
//                    Console.WriteLine("Log the error: Invalid data; store code not matching with current storecode.");
//                    continue;
//                }
//                int rowsAffected= SyncStoreTableData(storeModelObject);
//                if(rowsAffected > 0)
//                {
//                    Console.WriteLine("Log the Information: Storefile sync with DB is sucess.");
//                }
//                else
//                {
//                    Console.WriteLine("Log the Warning: Storefile sync with DB is not sucess.");
//                }

//                //string sourcePath = storeFilePath;
//                //string destPath = Path.Combine(storeDirectoryPath, "Processed", Path.GetFileName(storeFilePath));
//                //File.Move(sourcePath, destPath);


//                //----------------------------------------
//                /////Stock file Processing
//                //-------------------------------------------
//                //step-1 read data from file.
//                //step-2 validate the data
//                //step-3 update/insert/push the data to DB;

//                //read stock file
//                string[] stockFileContent = File.ReadAllLines(stockFilePath);
//                //validate file data
//                if (stockFileContent.Length < 1) 
//                {
//                    Console.WriteLine("Log the error:Invalid File");
//                }
//                for (int i = 1; i < stockFileContent.Length; i++)
//                {
//                    string[] stockFileData = stockFileContent[i].Split(';');



//                    if (stockFileContent.Length != 7)
//                    {
//                        Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 6 fileds data.");
//                        continue;
//                    }


//                    StockBO stockModelObject = new StockBO();

//                    stockModelObject.ProductCode = stockFileData[0];
//                    //stockModelObject.StoreCode = stockFileData[1];
//                    stockModelObject.ProductName = stockFileData[2];
//                    stockModelObject.QuantityAvailable = Convert.ToDecimal(stockFileData[3]);
//                    stockModelObject.Date = Convert.ToDateTime(stockFileData[4]);
//                    stockModelObject.PricePerUnit = Convert.ToDecimal(stockFileData[5]);




//                    if (!Products.Exists(x=>x.ProductCode==stockFileData[0]))
//                    {

//                        int rowsAffected1= SyncProductMasterTableData(stockModelObject);

//                    }
//                    int rowsAffected2 = SyncStockTableData(stockModelObject);
//                    if (rowsAffected2 > 0)
//                    {
//                        Console.WriteLine("Log the Information: Stockfile data sync with DB is sucess.");
//                    }
//                    else
//                    {
//                        Console.WriteLine("Log the Warning: Stockfile data sync with DB is not sucess.");
//                    }
//                }
//                //string sourcePath1 = stockFilePath;
//                //string destPath1 = Path.Combine(storeDirectoryPath, "Processed", Path.GetFileName(stockFilePath));
//                //File.Move(sourcePath1, destPath1);



//                //----------------------------------------
//                /////Employee file Processing
//                //-------------------------------------------
//                //step-1 read data from file.
//                //step-2 validate the data
//                //step-3 update/insert/push the data to DB;

//                //read stock file
//                string[] employeeFileContent = File.ReadAllLines(employeeFilePath);
//                if (employeeFileContent.Length < 1)
//                {
//                    Console.WriteLine("Log the error:Invalid File");

//                }
//                else if (employeeFileContent.Length == 1) 
//                {
//                    Console.WriteLine("Log the warning: No data present in the file");
//                    continue;
//                }
//                for (int i = 1; i < employeeFileContent.Length; i++)
//                {
//                    string[] employeeFileData = employeeFileContent[1].Split('|');
//                    if (employeeFileData.Length != 9)
//                    {
//                        Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 9 fileds data.");

//                    }
//                    EmployeeDTO employeeModelObject = new EmployeeDTO();
//                    //EmployeeCode|StoreCode|EmployeeName|Role|DateOfJoining|DateOfLeaving|ContactNumber|Gender|Salary
//                    employeeModelObject.EmployeeCode = employeeFileData[0];
//                    employeeModelObject.StoreCode = employeeFileData[1];
//                    employeeModelObject.EmployeeName = employeeFileData[2];
//                    employeeModelObject.Role = employeeFileData[3];
//                    employeeModelObject.DateOfJoining = Convert.ToDateTime(employeeFileData[4]);
//                    //first check value is null or empty


//                    if (string.IsNullOrEmpty(employeeFileData[5])) 
//                    {
//                        // If the value is null or empty, set DateOfLeaving to null
//                        employeeModelObject.DateOfLeaving = null;

//                    }
//                    else
//                    {
//                        //otherwise,Convert value to DateTime
//                        employeeModelObject.DateOfLeaving = Convert.ToDateTime(employeeFileData[5]);
//                    }

//                    employeeModelObject.ContactNumber = employeeFileData[6];
//                    employeeModelObject.Gender = employeeFileData[7];
//                    employeeModelObject.Salary = Convert.ToDecimal(employeeFileData[8]);
//                    //if(employeeData.Exists(x=>x.EmployeeCode == employeeFileData[0]))
//                    //{
//                    //    int rowsAffectedFromEmployeeTable= SyncEmployeeInsertDateInTable(employeeModelObject);

//                    //}
//                    //int rowsAffectedFromEmployeeTableAfterUpdate = SyncEmployeeUpdateDataInTable(employeeModelObject);


//                    // int rowsAffected3 = SyncEmployeeTableData(model2);
//                    //if (rowsAffected3 > 0)
//                    //{
//                    //    Console.WriteLine("Log the Information: Storefile sync with DB is sucess.");
//                    //}
//                    //else
//                    //{
//                    //    Console.WriteLine("Log the Warning: Storefile sync with DB is not sucess.");
//                    //}

//                }

























//            }


//            Console.ReadLine();

//        }

//        private static bool ValdateStoreFolderInDB(string storeDirName)
//        {
//            //option:1
//            //using dataReader read data
//            //read data at time one record only
//            ////int storeId = 0;
//            ////using (SqlConnection connection = new SqlConnection(rscConnectedString))
//            ////{
//            ////    using (SqlCommand command = new SqlCommand())
//            ////    {
//            ////        command.CommandText = "select * from stores where storeCode='" + storeDirName + "'";
//            ////        command.Connection = connection;
//            ////        connection.Open();
//            ////        using (SqlDataReader reader = command.ExecuteReader())
//            ////        {
//            ////            while (reader.Read())
//            ////            {
//            ////                storeId = (int)reader[0];
//            ////                int StoreId = (int)reader["storeId"];
//            ////                string StoreCode = Convert.ToString(reader["storeCode"]);
//            ////                string StoreName = Convert.ToString(reader["storeName"]);
//            ////                string Location = Convert.ToString(reader["location"]);
//            ////                string ManagerName = Convert.ToString(reader["managerName"]);
//            ////                string ContactNumber = Convert.ToString(reader["storeContactNumber"]);

//            ////            }


//            ////        }
//            ////        connection.Close();
//            ////    }

//            ////}
//            ////return storeId > 0;

//            //option-2=>2.1 read by using dataTable
//            //using DataAdapter read the data
//            //Read all data at a time
//            {

//                using (SqlConnection connection1 = new SqlConnection(rscConnectedString))
//                {
//                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from stores where storeCode='" + storeDirName + "';select * from stores;", connection1))
//                    {
//                        //command.CommandText = "select * from stores where storeCode = '" + storeDirName + "';
//                        //command.Connection = connection;
//                       // dataAdapter class no need to write open and close connetion automatically open and close connetions
//                       //In dataAdapter i am writing two select quries but i am printing only one table so i am taking datatable.You need all tables means go to dataset.
//                       using (DataTable table=new DataTable())
//                       {
//                            //the DataAdapter (da) was initialized with a SQL query,
//                            //the Fill method executes that query against the database,
//                            //retrieves the results, and adds the data to the DataTable.
//                            dataAdapter.Fill(table);
//                            return table.Rows.Count > 0;
//                       }
//                    }
//                }
//            }


//            //potion:2.2=>using dataset
//            //here i am using dataset 

//            //{

//            //    using (SqlConnection connection2 = new SqlConnection(rscConnectedString))
//            //    {
//            //        using (SqlDataAdapter dataAdapter1 = new SqlDataAdapter("select * from stores where storeCode='" + storeDirName + "';select * from stores; select * from stores; select * from stores;", connection2))
//            //        {
//            //            //using this code used for you will get all tables data in data set.
//            //            //In above i am giving four quries so i will get four tables data.
//            //            using (DataSet set = new DataSet())
//            //            {
//            //                dataAdapter1.Fill(set);
//            //                // Check if there are any tables in the DataSet

//            //                if (set.Tables.Count > 0)
//            //                {
//            //                    foreach (DataTable table in set.Tables)
//            //                    {
//            //                        Console.WriteLine("Table Name: " + table.TableName);
//            //                        Console.WriteLine("Number of Columns: " + table.Columns.Count);

//            //                    }
//            //                    return true;

//            //                }
//            //                else
//            //                {
//            //                    return false;
//            //                }

//            //            }
//            //        }
//            //    }
//            //}










//        }
//        private static List<StoreModel> GetAllStoresDataFromDB()
//        { //return type List<storeModel>
//            List<StoreModel> stores = new List<StoreModel>();
//            using (SqlConnection con =new SqlConnection(rscConnectedString))
//            {
//                using (SqlCommand command =new SqlCommand("select storeIdPk,storeCode,StoreName,Location,ManagerName,StoreContactNumber from stores",con)) 
//                {
//                    con.Open();
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        while (reader.Read()) 
//                        {
//                            StoreModel model = new StoreModel();
//                            model.StoreIdPk = Convert.ToInt32(reader["StoreIdPK"]);
//                            model.StoreCode = Convert.ToString(reader["storeCode"]);
//                            model.StoreName = Convert.ToString(reader["StoreName"]);
//                            model.Location = Convert.ToString(reader["Location"]);
//                            model.ManagerName = Convert.ToString(reader["ManagerName"]);
//                            model.ContactNumber = Convert.ToString(reader["StoreContactNumber"]);
//                            stores.Add(model);  
//                        }

//                    }
//                }
//                con.Close();
//            }
//            return stores;
//        }

//        private static int SyncStoreTableData(StoreModel storeModelObject)
//        {
//            using (SqlConnection con = new SqlConnection(rscConnectedString))
//            {
//                string query = "update stores set storeName=@StoreName,Location=@Location,ManagerName=@ManagerName,StoreContactNumber=@ContactNumber where StoreCode=@StoreCode";

//                using (SqlCommand command = new SqlCommand())
//                {
//                    con.Open();
//                    command.CommandText = query;
//                    command.Connection = con;
//                    command.Parameters.Add("@StoreName", DbType.String).Value = storeModelObject.StoreName;
//                    command.Parameters.Add("@StoreCode", DbType.String).Value = storeModelObject.StoreCode;
//                    command.Parameters.Add("@Location", DbType.String).Value = storeModelObject.Location;
//                    command.Parameters.Add("@ManagerName", DbType.String).Value = storeModelObject.ManagerName;
//                    command.Parameters.Add("@ContactNumber", DbType.String).Value = storeModelObject.ContactNumber;
//                    int rowsAffected = command.ExecuteNonQuery();
//                    con.Close();
//                    return rowsAffected;
//                }
//            }
//        }

//        private static List<ProductMasterBO> GetrAllProductsFromDB()
//        {
//            List<ProductMasterBO> ProductCodes = new List<ProductMasterBO>();
//            using (SqlConnection con = new SqlConnection(rscConnectedString))
//            {
//                using (SqlCommand command = new SqlCommand("SELECT ProductIdPk,ProductCode,ProductName,PricePerUnit FROM ProductMaster;", con))
//                {
//                    con.Open();
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        while (reader.Read()) 
//                        {
//                            ProductMasterBO model = new ProductMasterBO();
//                            model.ProductIdPk = Convert.ToInt32(reader["ProductIdPk"]);
//                            model.ProductCode = Convert.ToString(reader["ProductCode"]);
//                            model.ProductName = Convert.ToString(reader["ProductName"]);

//                            model.PricePerUnit = Convert.ToDecimal(reader["PricePerUnit"]);
//                            ProductCodes.Add(model);
//                        }

//                    }
//                    con.Close();
//                }
//            }
//            return ProductCodes;


//        }
//        private static int SyncProductMasterTableData(StockBO stockModelObject)
//        {
//            int rowsAffected1 = 0;

//            using (SqlConnection connection2 = new SqlConnection(rscConnectedString))
//            {
//                using (SqlCommand command1 = new SqlCommand())
//                {
//                    command1.CommandText = "INSERT INTO ProductMaster (ProductId,ProductCode,ProductName,PricePerUnit)VALUES ((select max(ProductId)+1 from productmaster),@ProductCode, @ProductName, @PricePerUnit)";
//                    command1.Connection = connection2;
//                    connection2.Open();
//                    command1.Parameters.Add("@productCode", DbType.String).Value = stockModelObject.ProductCode;
//                    command1.Parameters.Add("@ProductName", DbType.String).Value = stockModelObject.ProductName;
//                    command1.Parameters.Add("@PricePerUnit", DbType.Decimal).Value = stockModelObject.PricePerUnit;
//                     rowsAffected1 = command1.ExecuteNonQuery();
//                    connection2.Close();
//                    return rowsAffected1;

//                }

//            }
//        }
//        private static int SyncStockTableData(StockBO stockModelObject)
//        {
//            using (SqlConnection connection3 = new SqlConnection(rscConnectedString))
//            {
//                using (SqlCommand command2 = new SqlCommand())
//                {
//                    command2.CommandText = "INSERT INTO stock ( ProductId,StoreId,QuantityAvailable, Date)VALUES ((select ProductId from ProductMaster where productCode=@productCode),(select storeId from stores where storeCode=@storeCode),@QuantityAvailable,CAST(@Date AS DATE))";
//                    command2.Connection = connection3;
//                    connection3.Open();

//                    command2.Parameters.Add("@QuantityAvailable", DbType.Decimal).Value = stockModelObject.QuantityAvailable;
//                    command2.Parameters.Add("@productCode", DbType.String).Value = stockModelObject.ProductCode;
//                    //command2.Parameters.Add("@storeCode", DbType.String).Value = stockModelObject.StoreCode;
//                    if (stockModelObject.Date != null)
//                    {
//                        command2.Parameters.Add("@Date", SqlDbType.Date).Value = stockModelObject.Date;
//                    }
//                    else
//                    {
//                        command2.Parameters.Add("@Date", DbType.Date).Value = DBNull.Value; // Handle null Date

//                    }
//                    int rowsAffected2 = 0;


//                    try
//                    {
//                        rowsAffected2 = command2.ExecuteNonQuery();


//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("SQL Error: " + ex.Message);


//                    }

//                    return rowsAffected2;
//                    connection3.Close();


//                }

//            }


//        }

//        private static List<EmployeeDTO> GetAllEmployeeDataFromDB()
//        {
//            List<EmployeeDTO> employeeData= new List<EmployeeDTO>();
//            using (SqlConnection connection = new SqlConnection(rscConnectedString))
//            {
//                using (SqlCommand command = new SqlCommand())
//                {////EmployeeCode|StoreCode|EmployeeName|Role|DateOfJoining|DateOfLeaving|ContactNumber|Gender|Salary|StoreId
//                    command.CommandText = "select EmployeeCode,EmployeeName,Role,DateOfJoining,DateOfLeaving,ContactNumber,Gender,Salary,StoreId from Employee";
//                    command.Connection= connection;
//                    connection.Open();
//                    using (SqlDataReader reader = command.ExecuteReader()) 
//                    {
//                        while (reader.Read()) 
//                        { 
//                            EmployeeDTO modelObjectForDB = new EmployeeDTO();
//                            //reader default type is object
//                            //so,we declare type compalsary

//                            modelObjectForDB.EmployeeCode = Convert.ToString(reader["EmployeeCode"]);

//                            modelObjectForDB.EmployeeName = Convert.ToString(reader["EmployeeName"]);
//                            modelObjectForDB.Role = Convert.ToString(reader["role"]);
//                            modelObjectForDB.DateOfJoining = Convert.ToDateTime(reader["DateOfJoining"]);
//                            modelObjectForDB.DateOfLeaving = Convert.ToDateTime(reader["DateOfLeaving"]);
//                            modelObjectForDB.ContactNumber = Convert.ToString(reader["ContactNumber"]);
//                            modelObjectForDB.Gender = Convert.ToString(reader["Gender"]);
//                            modelObjectForDB.Salary = Convert.ToDecimal(reader["Salary"]);
//                            modelObjectForDB.StoreIdFk = Convert.ToInt32(reader["StoreIdFk"]);
//                            employeeData.Add(modelObjectForDB);





//                        }

//                    }
//                    connection.Close();


//                }
//                return employeeData;


//            }





//        }

//        //private static int SyncEmployeeInsertDateInTable(employeeModel employeeModelObject)
//        //{
//        //    using(SqlConnection con=new SqlConnection(rscConnectedString))
//        //    {
//        //        using(SqlCommand command=new SqlCommand())
//        //        {
//        //            command.CommandText="Insert InTo Employee(EmployeeCode , StoreCode , EmployeeName , Role , DateOfJoining , DateOfLeaving , ContactNumber , Gender , Salary)" +
//        //                "value(@EmployeeCode,(select StoreId from Stores where StoreCode=@StoreCode),@EmployeeName , @Role , @DateOfJoining , @DateOfLeaving , @ContactNumber , @Gender , @Salary)";
//        //            command.Connection = con;
//        //            con.Open();
//        //            command.Parameters.Add("EmployeeCode",DbType.String).Value= employeeModelObject.EmployeeName;
//        //            //command.Parameters.Add("StoreCode",);
//        //            //return rows
//        //        }
//        //    }
//        //}
//private void GetAllStocksFromDB()
//{
//     stockDBData = new List<StockBO>();
//    using (SqlConnection con = new SqlConnection(rscConnectedString))
//    {
//        using (SqlCommand command = new SqlCommand("SELECT StockIdPk, ProductIdFk, StoreIdFk, Date, QuantityAvailable From Stock", con))
//        {
//            con.Open();
//            using (SqlDataReader reader = command.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    StockBO stockModel = new StockBO();
//                    stockModel.StockIdPk = Convert.ToInt32(reader["StockIdPk"]);
//                    stockModel.ProductIdFk= Convert.ToInt32(reader["ProductIdFk"]);

//                    stockModel.StoreIdFK = Convert.ToInt32(reader["StoreIdFk"]);
//                    stockModel.Date = Convert.ToDateTime(reader["Date"]);
//                    stockModel.QuantityAvailable = Convert.ToDecimal(reader["QuantityAvailable"]);


//                    stockDBData.Add(stockModel);


//                }

//            }
//            con.Close();
//        }
//    }

//}

//    }
//}



















