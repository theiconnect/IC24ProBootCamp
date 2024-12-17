using FileModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using FileModel;
using ProjectHelpers;
using Enum;
namespace DBDataAcesses
{
    public class PushDataIntoDb:BaseProcessor
    {
        public  static void PushStoreDataToDB(WareHouseModel wareHouseModel, bool isValidFile, string wareHouseFilePath)
        {
            if (!isValidFile)
            {
                FileHelper.MoiveFile(wareHouseFilePath,FileStatus.Failure);
                return;
            }


            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"UpdateWarehousesData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WareHouseCode", wareHouseModel.WareHouseCode);
                        cmd.Parameters.AddWithValue("@WareHouseName", wareHouseModel.WareHouseName);
                        cmd.Parameters.AddWithValue("@Location", wareHouseModel.Location);
                        cmd.Parameters.AddWithValue("@ManagerName", wareHouseModel.ManagerName);
                        cmd.Parameters.AddWithValue("@ContactNO", wareHouseModel.ContactNumber);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                FileHelper.MoiveFile(wareHouseFilePath, FileStatus.Success);

            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(wareHouseFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }

        }


        public static void PushEmployeeDataToDB(List<EmployeeModel> EmployeesList, string employeeFilePath)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertOrUpdateEmpData", conn))
                    {
                        foreach (var employeeData in EmployeesList)
                        {
                            if (!employeeData.IsValidEmpolyee) continue;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@EmpCode", employeeData.EmpCode);
                            cmd.Parameters.AddWithValue("@EmpName", employeeData.EmpName);
                            cmd.Parameters.AddWithValue("@EmpWareHouseCode", employeeData.EmpWareHouseCode);
                            cmd.Parameters.AddWithValue("@EmpContactNumber", employeeData.EmpContactNumber);
                            cmd.Parameters.AddWithValue("@Gender", employeeData.Gender);
                            cmd.Parameters.AddWithValue("@Salary", employeeData.Salary);
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                FileHelper.MoiveFile(employeeFilePath, FileStatus.Success);


            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(employeeFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }

        }



        public static void PushCustomerDataToDB(List<CustomerModel> Customers, int wareHouseId, string customerFilePath, string ordersFilePath, string orderItemFilePath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {

                        command.CommandText = "UpdateOrInsertCustomer";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        foreach (var customerRecord in Customers)
                        {
                            if (!customerRecord.IsValidCustomer) continue;
                            command.Parameters.Clear();
                            command.Parameters.Add("@CustomerName", DbType.String).Value = customerRecord.CustomerName;
                            command.Parameters.Add("@PhNo", DbType.String).Value = customerRecord.ContactNumber;
                            command.ExecuteNonQuery();

                        }

                        connection.Close();


                    }
                }

                FileHelper.MoiveFile(customerFilePath, FileStatus.Success);

            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(customerFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }

            PushOrderDataToDB(Customers, wareHouseId, ordersFilePath, orderItemFilePath);

        }
        private static void PushOrderDataToDB(List<CustomerModel> Customers, int wareHouseId, string ordersFilePath, string orderItemFilePath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "AddOrder";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        foreach (var cust in Customers)
                        {
                            if (!cust.IsValidCustomer) continue;
                            foreach (var OrderRecord in cust.Orders)
                            {
                                if (!OrderRecord.IsValidOrder) continue;

                                command.Parameters.Clear();
                                command.Parameters.Add("@InvoiceNumber", DbType.String).Value = OrderRecord.InvoiceNumber;
                                command.Parameters.Add("@PhNo", DbType.String).Value = OrderRecord.CustomerPhNo;
                                command.Parameters.Add("@WareHouseIdfk", DbType.Int32).Value = wareHouseId;
                                command.Parameters.Add("@OrderDate", DbType.DateTime).Value = OrderRecord.Date;
                                command.Parameters.Add("@NoOfItems", DbType.Int32).Value = OrderRecord.NoOfItems;
                                command.Parameters.Add("@PaymentStatus", DbType.String).Value = OrderRecord.PaymentStaus;
                                command.Parameters.Add("@TotalAmount", DbType.Decimal).Value = OrderRecord.TotalAmount;

                                command.ExecuteNonQuery();


                            }
                        }

                        connection.Close();
                    }
                    FileHelper.MoiveFile(ordersFilePath, FileStatus.Success);

                    PushOrderItemDataToDB(Customers, wareHouseId, orderItemFilePath);
                }
            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(ordersFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }
        }
        private static void PushOrderItemDataToDB(List<CustomerModel> Customers, int wareHouseId, string orderItemFilePath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "AddOrderItem";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        foreach (var cust in Customers)
                        {
                            if (!cust.IsValidCustomer) continue;
                            foreach (var order in cust.Orders)

                            {
                                if (!order.IsValidOrder) continue;
                                foreach (var OrderItem in order.Items)
                                {
                                    if (!OrderItem.IsValidItem) continue;
                                    command.Parameters.Clear();

                                    // Add parameters with correct DbType and values
                                    command.Parameters.Add("@InvoiceNumber", DbType.String).Value = OrderItem.InvoiceNumber;
                                    command.Parameters.Add("@ProductCode", DbType.String).Value = OrderItem.ProductCode;
                                    command.Parameters.Add("@WareHouseIdfk", DbType.Int32).Value = wareHouseId;
                                    command.Parameters.Add("@Quantity", DbType.Decimal).Value = OrderItem.Quantity;
                                    command.Parameters.Add("@PricePerUnit", DbType.Decimal).Value = OrderItem.PricePerUnit;
                                    command.Parameters.Add("@TotalAmount", DbType.Decimal).Value = OrderItem.TotalAmount;
                                    command.ExecuteNonQuery();

                                }

                            }
                        }

                        connection.Close();
                    }

                }
                FileHelper.MoiveFile(orderItemFilePath, FileStatus.Success);


            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(orderItemFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }


        }


        public static void PushRetrunsDataToDB(List<ReturnsModel> returnsList, string returnFilePath)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "AddReturn";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        foreach (var returnRecord in returnsList)
                        {
                            if (!returnRecord.IsvalidReturn) continue;
                            cmd.Parameters.Clear();

                            // Add parameters with correct DbType and values
                            cmd.Parameters.Add(new SqlParameter("@ReturnDate", SqlDbType.DateTime) { Value = returnRecord.Date });
                            cmd.Parameters.Add(new SqlParameter("@InvoiceNumber", SqlDbType.NVarChar, 50) { Value = returnRecord.InvoiceNumber });
                            cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar, 255) { Value = returnRecord.Reason });
                            cmd.Parameters.Add(new SqlParameter("@ReturnStatus", SqlDbType.NVarChar, 50) { Value = returnRecord.ReturnStatus });
                            cmd.Parameters.Add(new SqlParameter("@AmountRefund", SqlDbType.Decimal) { Value = returnRecord.AmountRefund });

                            // Execute the stored procedure
                            cmd.ExecuteNonQuery();

                        }
                        conn.Close();
                    }
                }


                FileHelper.MoiveFile(returnFilePath, FileStatus.Success);

            }
            catch (Exception ex)
            {

                FileHelper.MoiveFile(returnFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }

        }


        public static void PushInvetoryDataToDB(string failedReason, List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList, string dirName, string stockDateStr, DateTime date, string inventoryPath)
        {

            if (!string.IsNullOrEmpty(failedReason))
            {
                return;
            }

            SyncProducts(inventoryList, dBStockDatas, productMasterList);

            dBStockDatas=GetAllStockInfoOfTodayFromDB(dBStockDatas, date);

            SyncStockFileWithDB(inventoryList, dBStockDatas, dirName, stockDateStr, inventoryPath);


        }

        private static void SyncProducts(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, List<ProductMasterModel> productMasterList)
        {
            string query = string.Empty;

             productMasterList= GetAllProductsFromDB(productMasterList);
            try
            {
                foreach (var stock in inventoryList)
                {

                    var product = productMasterList.Find(x => x.ProductCode == stock.productCode);
                    if (product == null)
                    {
                        query = query + "insert into Products(ProductCode, ProductName, PricePerUnit,categoryidfk)" +
                            $"   Values('{stock.productCode}', '{stock.productCode}', {stock.pricePerUnit},{1})" +
                            $"";
                    }
                    else
                    {
                        stock.ProductIdFk = product.ProductIdPk;
                    }
                }
                if (!string.IsNullOrEmpty(query))
                {
                    using (SqlConnection con = new SqlConnection(oMSConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    productMasterList= GetAllProductsFromDB(productMasterList);
                    foreach (var stock in inventoryList)
                    {
                        if (Convert.ToInt32(stock.ProductIdFk) == 0)
                        {
                            var product = productMasterList.FirstOrDefault(x => x.ProductCode == stock.productCode);
                            stock.ProductIdFk = product.ProductIdPk;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static List<ProductMasterModel> GetAllProductsFromDB(List<ProductMasterModel> productMasterList)
        {
            productMasterList = new List<ProductMasterModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllProductsFromDB", conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ProductMasterModel model = new ProductMasterModel();
                                model.ProductIdPk = reader.GetInt32(0);
                                model.ProductIdPk = Convert.ToInt32(reader["Productidpk"]);
                                model.ProductCode = reader.GetString(1);
                                model.ProductName = reader.GetString(2);
                                model.PricePerUnit = reader.GetDecimal(3);
                                productMasterList.Add(model);
                            }
                        }
                        conn.Close();

                    }


                }
                return productMasterList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        private static List<DBStockData> GetAllStockInfoOfTodayFromDB(List<DBStockData> dBStockDatas, DateTime date)
        {
            dBStockDatas = new List<DBStockData>();
            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand($"GetAllStockInfoOfTodayFromDB", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@StockDateStr", DbType.Date).Value = Convert.ToDateTime(StockDateStr);
                        cmd.Parameters.Add(new SqlParameter("@StockDateStr", SqlDbType.DateTime) { Value = date });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DBStockData model = new DBStockData();
                                model.InventoryIdPk = reader.GetInt32(0);
                                model.WareHouseIdFk = reader.GetInt32(1);
                                model.ProductIdFk = reader.GetInt32(2);
                                model.Date = reader.GetDateTime(3);
                                model.QuantityAvailable = reader.GetDecimal(4);
                                model.PricePerUnit = reader.GetDecimal(5);
                                model.RemainingQuantity = reader.GetDecimal(6);
                                dBStockDatas.Add(model);
                            }
                        }
                    }
                }
                return dBStockDatas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        private static void SyncStockFileWithDB(List<InventoryModel> inventoryList, List<DBStockData> dBStockDatas, string dirName, string stockDateStr, string inventoryPath)
        {
            try
            {

                foreach (var stock in inventoryList)
                {

                    if (!dBStockDatas.Exists(s => s.Date == stock.date && s.ProductIdFk == stock.ProductIdFk))
                    {
                        using (SqlConnection con = new SqlConnection(oMSConnectionString))
                        {
                            con.Open();

                            using (SqlCommand cmd = new SqlCommand("AddInventory", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@WarehouseCode", dirName);
                                cmd.Parameters.AddWithValue("@ProductIdFk", stock.ProductIdFk);
                                cmd.Parameters.AddWithValue("@StockDate", stockDateStr);
                                cmd.Parameters.AddWithValue("@AvailableQuantity", stock.availableQuantity);
                                cmd.Parameters.AddWithValue("@PricePerUnit", stock.pricePerUnit);
                                cmd.Parameters.AddWithValue("@RemainingQuantity", stock.remainingQuantity);
                                cmd.ExecuteNonQuery();
                            }
                            con.Close();
                        }
                    }
                }
                FileHelper.MoiveFile(inventoryPath, FileStatus.Success);

            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(inventoryPath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }
        }



    }
}
