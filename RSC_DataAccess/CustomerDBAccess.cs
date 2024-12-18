using RSC_Configurations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Models;

namespace RSC_DataAccess
{
    public class CustomerDBAccess
    {
        private int Storeid { get; set; }
        public  CustomerDBAccess(List<CustumerModel> custumerData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
                {
                    con.Open();
                    string StoreProcedure = "PushCustomerDataToDB";
                    using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                    {
                        foreach (var Empdata in custumerData)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@customercode", Empdata.CustomerCode);
                            cmd.Parameters.AddWithValue("@name", Empdata.CustomerName);
                            cmd.Parameters.AddWithValue("@email", Empdata.CustomerEmail);
                            cmd.Parameters.AddWithValue("@contactnumber", Empdata.ContactNumber);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CustomerOrderPushToDB(List<CustumerModel> custumerData, int storeid)
        {
            Storeid = storeid;  
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
                {
                    con.Open();
                    string storeProcedure = "customerOrderDataToDB";
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        foreach (var custumer in custumerData)
                        {
                            if (!custumer.IsValidCustomer)
                            {
                                foreach (var CustomerOrder in custumer.custumerOrders)
                                {
                                    if (!CustomerOrder.IsValidOrder)
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ordercode", CustomerOrder.OrderCode);
                                        cmd.Parameters.AddWithValue("@storeidfk", this.Storeid);
                                        cmd.Parameters.AddWithValue("@orderdata", CustomerOrder.OrderDatestr);
                                        cmd.Parameters.AddWithValue("@noofitems", CustomerOrder.NoFoIteams);
                                        cmd.Parameters.AddWithValue("@amount", CustomerOrder.Amount);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                        }

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void pushOrderBillingDataToDB(List<CustumerModel> custumerData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
                {
                    con.Open();
                    string StoreProcedure = "OrderBillingDataToDB";
                    using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                    {
                        foreach (var custumer in custumerData)
                        {
                            if (!custumer.IsValidCustomer) continue;

                            foreach (var order in custumer.custumerOrders)
                            {
                                if (!order.IsValidOrder) continue;

                                foreach (var billing in order.OrderBillings)
                                {
                                    if (billing.IsValidBilling) continue;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@billnumber", billing.BillingNumber);
                                    cmd.Parameters.AddWithValue("@paymentmode", billing.ModeOfPayment);
                                    cmd.Parameters.AddWithValue("@billingdate", billing.BillingDate);
                                    cmd.Parameters.AddWithValue("@amount", billing.Amount);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
