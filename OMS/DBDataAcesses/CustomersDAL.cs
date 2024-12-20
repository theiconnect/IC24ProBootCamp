using Configuration;
using Enum;
using FileModel;
using ProjectHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using OMS_IDAL;

namespace DBDataAcesses
{
    public class CustomersDAL:BaseProcessor,ICustomerDAL
    {
        public  void PushCustomerDataToDB(List<CustomerModel> Customers, int wareHouseId, string customerFilePath, string ordersFilePath, string orderItemFilePath)
        {
            SqlConnection conn = null;
            try
            {
                using ( conn = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {

                        command.CommandText = "UpdateOrInsertCustomer";
                        command.Connection = conn;
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        foreach (var customerRecord in Customers)
                        {
                            if (!customerRecord.IsValidCustomer) continue;
                            command.Parameters.Clear();
                            command.Parameters.Add("@CustomerName", DbType.String).Value = customerRecord.CustomerName;
                            command.Parameters.Add("@PhNo", DbType.String).Value = customerRecord.ContactNumber;
                            command.ExecuteNonQuery();

                        }

                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }

                   
                }

                FileHelper.MoiveFile(customerFilePath, FileStatus.Success);

            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(customerFilePath, FileStatus.Failure);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }

            PushOrderDataToDB(Customers, wareHouseId, ordersFilePath, orderItemFilePath);

        }
        public  void PushOrderDataToDB(List<CustomerModel> Customers, int wareHouseId, string ordersFilePath, string orderItemFilePath)
        {
            SqlConnection conn=null;
            try
            {
                using ( conn = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "AddOrder";
                        command.Connection = conn;
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();

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
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }

                    }
                    FileHelper.MoiveFile(ordersFilePath, FileStatus.Success);

                    PushOrderItemDataToDB(Customers, wareHouseId, orderItemFilePath);
                }
            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(ordersFilePath, FileStatus.Failure);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }
        public    void PushOrderItemDataToDB(List<CustomerModel> Customers, int wareHouseId, string orderItemFilePath)
        {
            SqlConnection conn=null;
            try
            {
                using ( conn = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "AddOrderItem";
                        command.Connection = conn;
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
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

                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }

                }
                FileHelper.MoiveFile(orderItemFilePath, FileStatus.Success);


            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(orderItemFilePath, FileStatus.Failure);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }


        }

    }
}
