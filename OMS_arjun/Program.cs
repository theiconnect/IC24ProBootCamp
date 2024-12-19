using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using OMS_arjun.Models;

namespace OMS_arjun
{
    internal class Program
    {
        static string rootfolderPath { get; set; }
        static string oMSConnectionString { get; set; }
        static Program()
        {
            rootfolderPath = ConfigurationManager.AppSettings["RootLocalFolder"];
            oMSConnectionString = ConfigurationManager.ConnectionStrings["iConnectOMSConnectionString"].ToString();

        }
        static void Main(string[] args)
        {            
                string[] WareHouseFolder = Directory.GetDirectories(rootfolderPath);
                
                List<WareHouseModel> WareHouses = GetAllWareHousesFromDB();
                List<EmployeeModel> Emp = new List<EmployeeModel>();
                foreach (var WareHouseFolderPath in WareHouseFolder)
                {
                    string WareHouseFolderName = Path.GetFileName(WareHouseFolderPath);
                    if (!(WareHouses.Exists(x => x.WareHouseCode == WareHouseFolderName)))
                    {
                        //Ignore the folder
                        continue;
                    }
                    string[] wareHouseFiles = Directory.GetFiles(WareHouseFolderPath);
                    string wareHouseFile = string.Empty;
                    string employeeFile = string.Empty;
                    string inventoryFile = string.Empty;
                    string customersFile = string.Empty;
                    string ordersFile = string.Empty;
                    string orderItmesFile = string.Empty;
                    string ReturnsFile = string.Empty;
                    foreach (var filePaths in wareHouseFiles)
                    {
                        if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("warehouse_"))
                        {
                            wareHouseFile = filePaths;
                        }
                        else if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("employee_"))
                        {
                            employeeFile = filePaths;
                        }
                        else if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("inventory_"))
                        {
                            inventoryFile = filePaths;
                        }
                        else if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("customers_"))
                        {
                            customersFile = filePaths;
                        }
                        else if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("orders_"))
                        {
                            ordersFile = filePaths;
                        }
                        else if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("orderitem_"))
                        {
                            orderItmesFile = filePaths;
                        }
                        else if (Path.GetFileNameWithoutExtension(filePaths).Trim().ToLower().StartsWith("returns_"))
                        {
                            ReturnsFile = filePaths;
                        }
                        else
                        {
                            Console.WriteLine("invalidfile");
                        }
                    }

                }

                Console.ReadLine();
        }
        

        private static void processingCustomers(string customersFile, string WareHouseFolderPath)
        {
            List<CustomerModel> customerModels = new List<CustomerModel>();
            string[] customerContent = File.ReadAllLines(customersFile);
            DataTable dataTable = new DataTable();


            for (int i = 1; i < customerContent.Length; i++)
            {
                CustomerModel model = new CustomerModel();
                char[] chars = { '|', ',' };
                string[] dataOfCustomers = customerContent[i].Split(chars);

                model.CustomerName = dataOfCustomers[0];
                model.ContactNumber = dataOfCustomers[1];
                customerModels.Add(model);

            }
            syncCustomerData(customerModels);
        }



        private static void syncCustomerData(List<CustomerModel> customerModels)
        {
            using (SqlConnection conn = new SqlConnection(oMSConnectionString))
            {
                conn.Open();
                foreach (CustomerModel model in customerModels)
                {

                    using (SqlCommand cmd = new SqlCommand($"if  exists (select customername from customers where phno='{model.ContactNumber}') " +
                        $"begin " +
                        $"update customers set customername='{model.CustomerName}', phno='{model.ContactNumber}'" +
                        $"where phno='{model.ContactNumber}'" +
                        $" end " +
                        $" else " +
                        $" begin " +
                        $" insert into customers " +
                        $" values ('{model.CustomerName}','{model.ContactNumber}') " +
                        $" end ", conn

                        ))

                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }
        private static void ProcessEmployee(string employeeFile, string wareHouseFolderPath)
        {
            bool emptyFile = true;
            using (XmlReader reader = XmlReader.Create(employeeFile))
            {
                EmployeeModel models = new EmployeeModel();
                int move = 0;
                try
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement()) 
                        {
                            emptyFile = false;
                            if (reader.Name == "EmployeeCode")
                            {
                               
                                models.EmpCode = reader.ReadElementContentAsString();
                                if (models.EmpCode == "")
                                {
                                    Console.WriteLine("Invalid employee record: EmployeeCode cannot be empty");
                                    break;
                                }

                            }
                            else if (reader.Name == "EmployeeName")
                            {
                                models.EmpName = reader.ReadElementContentAsString();
                                break;
                            }
                            else if (reader.Name == "WarehouseCode")
                            {
                                models.AddreWareHouseIdfkss = reader.ReadElementContentAsString();
                                if (models.AddreWareHouseIdfkss == "")
                                {
                                    break;
                                }
                            }
                            else if (reader.Name == "ContactNumber")
                            {
                                models.ContactNumber = reader.ReadElementContentAsString();
                                if (models.ContactNumber == "")
                                {
                                    break;
                                }
                            }
                            else if ((reader.Name == "Gender"))
                            {
                                models.Gender = reader.ReadElementContentAsString();
                                if (models.Gender == "")
                                {
                                    break;
                                }
                            }
                            else if (reader.Name == "Salary")
                            {
                                models.Salary = reader.ReadElementContentAsString();
                                if (models.Salary == "")
                                {
                                    break;
                                }
                                int rowseffected = syncEmployeedata(models);


                            }
                        }


                    }
                    if (move > 0)
                    {
                        File.Move(employeeFile, Path.Combine(wareHouseFolderPath, "processed", Path.GetFileName(employeeFile)));
                    }
                }
                catch (Exception ex)
                {

                    emptyFile = true;
                }


            }

            if (emptyFile)
            {
                File.Move(employeeFile, Path.Combine(wareHouseFolderPath, "archive", Path.GetFileName(employeeFile)));
            }
        }

        private static void ProcessWareHouse(string wareHouseFile, string wareHouseFolderPath, string wareHouseFolderName)
        {
            string[] dataOfFile = File.ReadAllLines(wareHouseFile);
            if (dataOfFile.Length < 1)
            {
                Console.WriteLine("Log the file: invalid file");
            }
            else if (dataOfFile.Length == 1)
            {
                Console.WriteLine("Log the file: No data present in the file");
            }
            else if (dataOfFile.Length > 2)
            {
                Console.WriteLine("Log the file: Records are morethan one");
            }
            else if (dataOfFile.Length == 2)
            {
                char[] chars = { ',', '|' };
                string[] data = dataOfFile[1].Split(chars);
                if (data.Length != 5)
                {
                    Console.WriteLine("invalid file");
                    File.Move(wareHouseFile, Path.Combine(wareHouseFolderPath + "archive", Path.GetFileName(wareHouseFile)));

                }
                else
                {
                    WareHouseModel models = new WareHouseModel();
                    models.WareHouseCode = data[0];
                    models.WareHouseName = data[1];
                    models.Location = data[2];
                    models.ManagerName = data[3];
                    models.ContactNo = data[4];

                    if (models.WareHouseCode != wareHouseFolderName)
                    {
                        Console.WriteLine("warehouse code doesn't match the current context");
                        File.Move(wareHouseFolderPath, Path.Combine(wareHouseFolderPath + "archive", Path.GetFileName(wareHouseFile)));


                    }
                    else
                    {
                        int rowseffected = syncWareHousedata(models);
                        if (rowseffected > 0)
                        {

                            Console.WriteLine("Log the Information: Storefile sync with DB is sucess.");
                        }
                        else
                        {
                            Console.WriteLine("Log the Warning: Storefile sync with DB is not sucess.");

                        }
                        File.Move(wareHouseFile, Path.Combine(wareHouseFolderPath, "processed", Path.GetFileName(wareHouseFile)));

                    }


                }

            }

        }

        private static int syncEmployeedata(EmployeeModel models)
        {
            using (SqlConnection conn = new SqlConnection(oMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"if not  exists (select empname from Employee where empcode ='{models.EmpCode}') " +
                    $"begin " +
                    $"insert into Employee(empcode,EmpName,WareHouseIdfk,ContactNumber,Gender,Salary)" +
                    $"values ('{models.EmpCode}','{models.EmpName}',(select warehouseidpk from warehouse where warehousecode='{models.AddreWareHouseIdfkss}')," +
                    $"'{models.ContactNumber}'," +
                    $"'{models.Gender}','{models.Salary}')" +
                    $" end  " +

                    $"else " +

                    $" begin " +
                    $"update Employee  " +
                    $"set empcode='{models.EmpCode}',empname='{models.EmpName}', warehouseidfk=(select warehouseidpk from warehouse where warehousecode='{models.AddreWareHouseIdfkss}'), contactnumber='{models.ContactNumber}', gender='{models.Gender}',  Salary='{models.Salary}' " +
                    $"where  empcode='{models.EmpCode}'" +
                    $"end", conn))
                {
                    conn.Open();
                    int rowsaffected = cmd.ExecuteNonQuery();

                    conn.Close();
                    return rowsaffected;

                }
            }
        }

        private static int syncWareHousedata(WareHouseModel models)
        {
            using (SqlConnection conn = new SqlConnection(oMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"Update warehouse Set warehouseName = '{models.WareHouseName}', Location = '{models.Location}', ManagerName= '{models.ManagerName}', ContactNo = '{models.ContactNo}' where WareHouseCode = '{models.WareHouseCode}'", conn))
                {
                    conn.Open();
                    int rowsaffected = cmd.ExecuteNonQuery();

                    conn.Close();
                    return rowsaffected;

                }
            }
        }

        private static List<WareHouseModel> GetAllWareHousesFromDB()
        {
            List<WareHouseModel> wareHouses = new List<WareHouseModel>();

            using (SqlConnection con = new SqlConnection(oMSConnectionString))

            {
                using (SqlCommand cmd = new SqlCommand("SELECT WarehouseIdPk,WarehouseCode,WarehouseName,Location,ManagerName,ContactNo FROM Warehouse", con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        WareHouseModel models = new WareHouseModel();
                        models.WareHouseIdpk = Convert.ToInt32(reader["WarehouseIdPk"]);
                        models.WareHouseCode = Convert.ToString(reader["WarehouseCode"]);
                        models.WareHouseName = Convert.ToString(reader["WarehouseName"]);
                        models.Location = Convert.ToString(reader["Location"]);
                        models.ManagerName = Convert.ToString(reader["ManagerName"]);
                        models.ContactNo = Convert.ToString(reader["ContactNo"]);

                        wareHouses.Add(models);
                    }

                }
                con.Close();
                return wareHouses;

            }

        }
    }
}
