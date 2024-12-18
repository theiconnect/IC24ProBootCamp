using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using PathAndDataBaseConfig;

namespace DataAccess
{
    public class CustomerDA
    {
        public void SyncCustomerData(List<CustomerModel> customers)
        {
            SyncCustomerDataWithDB(customers);
            SyncOrderDataWithDB(customers);
            SyncBillingDataWithDB(customers);

        }

        private void SyncCustomerDataWithDB(List<CustomerModel> customers)
        {
            using (SqlConnection connetion = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        //"If exists(select ContactNumber from Customer where ContactNUmber=@ContactNumber)" +
                        //"\r\nBegin\r\nupdate Customer set name=@name,email=@email,ContactNumber=@ContactNumber," +
                        //"customerCode=@customerCode; \r\nend\r\nelse\r\nbegin\r\nInsert InTo Customer(Name,Email," +
                        //"ContactNumber,CustomerCode)\r\nvalues(@Name,@Email,@ContactNumber,@CustomerCode);\r\nend\r\n";
                        command.CommandText = "InsertOrUpdateCustomer";
                        command.Connection = connetion;
                        command.CommandType = CommandType.StoredProcedure;
                        connetion.Open();
                        foreach (var customerRecord in customers)
                        {
                            customerRecord.IsValidCustomer = true;
                            command.Parameters.Add("@Name", DbType.String).Value = customerRecord.Name;
                            command.Parameters.Add("@Email", DbType.String).Value = customerRecord.Email;
                            command.Parameters.Add("@ContactNumber", DbType.String).Value = customerRecord.ContactNumber;
                            command.Parameters.Add("@CustomerCode", DbType.String).Value = customerRecord.CustomerCode;
                            //command.ExecuteNonQuery();
                        }
                        
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Error:" + ex.Message);
                    throw;
                }
                finally
                {
                    if (connetion.State == ConnectionState.Open)
                    {
                        connetion.Close();

                    }
                }



            }

        }

        private void SyncOrderDataWithDB(List<CustomerModel> customers)
        {
            using (SqlConnection connetion = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        //"Insert InTo Orders(OrderDate,StoreIdFk,CustomerIdFk,EmployeeIdFk,OrderCode,NoOfItems,ToTalAmount)" +
                        //"values(@OrderDate,(select storeIdPk from stores where storeCode=@storeCode)," +
                        //"(select CustomerIdPk from Customer where CustomerCode=@CustomerCode)," +
                        //"(select EmployeeIdPk from Employee where EmployeeCode=@EmployeeCode),@OrderCode,@NoOfItems,@ToTalAmount)";
                        command.CommandText = "InsertOrders";
                        command.Connection = connetion;
                        command.CommandType = CommandType.StoredProcedure;
                        connetion.Open();
                        foreach (var customerRecord in customers)
                        {
                            foreach (var orderRecord in customerRecord.CustomerOrders)
                            {
                                orderRecord.IsValidOrder = true;
                                command.Parameters.Add("@orderDate", DbType.DateTime).Value =                orderRecord.OrderDate;
                                command.Parameters.Add("@StoreCode", DbType.String).Value = orderRecord.StoreCode;
                                command.Parameters.Add("@CustomerCode", DbType.String).Value = orderRecord.CustomerCode;
                                command.Parameters.Add("@EmployeeCode", DbType.String).Value = orderRecord.EmployeeCode;
                                command.Parameters.Add("@OrderCode", DbType.String).Value = orderRecord.OrderCode;
                                command.Parameters.Add("@NoOfItems", DbType.String).Value = orderRecord.NoOfItems;
                                command.Parameters.Add("@TotalAmount", DbType.Decimal).Value = orderRecord.Amount;
                                command.ExecuteNonQuery();
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    throw;

                }
                finally
                {
                    if (connetion.State == ConnectionState.Open)
                    {
                        connetion.Close();

                    }
                }
                

            }

        }
        private void SyncBillingDataWithDB(List<CustomerModel> customers)
        {
            using (SqlConnection connection = new SqlConnection(BaseProcessor.rscConnectedString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        //"Insert InTo Billing(BillingIdPk,BillingNumber,BillingDate,PaymentMode,OrderIdFk,Amount)" +
                        //"values(@BillingIdPk,@BillingNumber,@BillingDate,@PaymentMode,(select OrderIdPk from orders where OrderCode=@OrderCode),@Amount)";
                        command.CommandText = "InsertBilling";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        foreach (var customerRecord in customers)
                        {
                            foreach (var orderRecord in customerRecord.CustomerOrders)
                            {
                                foreach (var billingRecord in orderRecord.OrderBilling)
                                {
                                    command.Parameters.Add("@BillingIdPk", DbType.Int32).Value =                              billingRecord.BillingIdPk;
                                    command.Parameters.Add("@BillingNumber", DbType.String).Value =                           billingRecord.BillingNumber;
                                    command.Parameters.Add("@BillingDate", DbType.DateTime).Value =                           billingRecord.BillingDate;
                                    command.Parameters.Add("@PaymentMode", DbType.Decimal).Value =                            billingRecord.ModeOfPayment;
                                    command.Parameters.Add("@OrderCode", DbType.String).Value =                               billingRecord.OrderCode;
                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    throw;

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                
            }

        }
    }
}
