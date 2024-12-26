using RSC_Configurations;
using RSC_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_IDAL;
using RSC_EntityDAL.EF;
using System.Runtime.InteropServices;

namespace RSC_EntityDAL
{
    public class CustomerEntityDAL:ICustomerDAL
    {
        private int Storeid { get; set; }
        private RSCEntities RSCDB { get; set; }

        public CustomerEntityDAL()
        {
            RSCDB = new RSCEntities();
        }
        public void CustomerDBAcces(List<CustumerModel> custumerData, int storeid)
        {
            this.Storeid = storeid;
            foreach(var Customerdata in custumerData)
            {
                if (!Customerdata.IsValidCustomer)
                {
                    var CustomerDt = RSCDB.Customers.FirstOrDefault(c => c.CustomerCode == Customerdata.CustomerCode && c.ContactNumber == Customerdata.ContactNumber);
                    if (CustomerDt != null)
                    {
                        CustomerDt.Name = Customerdata.CustomerName;
                        CustomerDt.Email = Customerdata.CustomerEmail;
                        RSCDB.SaveChanges();
                    }
                    else
                    {
                        Customers CustomerData = new Customers();
                        CustomerData.ContactNumber = Customerdata.ContactNumber;
                        CustomerData.CustomerCode = Customerdata.CustomerCode;
                        CustomerData.Email = Customerdata.CustomerEmail;
                        CustomerData.Name = Customerdata.CustomerName;
                        RSCDB.Customers.Add(CustomerData);
                        RSCDB.SaveChanges();
                    }
                }
            }
        }

      
        public void CustomerOrderPushToDB(List<CustumerModel> custumerData)
        {
            foreach(var Customer in custumerData)
            {
                if (!Customer.IsValidCustomer)
                {
                    foreach (var CustomerOrder in Customer.custumerOrders)
                    {
                        if (!CustomerOrder.IsValidOrder)
                        {
                            var CustomerID = RSCDB.Customers.Where(c => c.CustomerCode == CustomerOrder.CustomerCode).Select(c => c.CustomerIdPk).FirstOrDefault();
                            var EmployeeID = RSCDB.Employees.Where(E => E.EmpCode == CustomerOrder.EmployeeCode).Select(E => E.EmployeeIdPk).FirstOrDefault();
                            var OrderData = RSCDB.Orders.FirstOrDefault(O => O.CustomerIdFk == CustomerID);
                            if (OrderData != null)
                            {
                                OrderData.OrderDate = CustomerOrder.OrderDate;
                                OrderData.StoreIdFk = this.Storeid;
                                OrderData.NoOfItems = CustomerOrder.NoFoIteams;
                                OrderData.EmployeeIdFk = EmployeeID;
                                OrderData.Amount = CustomerOrder.Amount;
                                OrderData.OrderCode = CustomerOrder.OrderCode;
                                RSCDB.SaveChanges();
                            }
                            else
                            {
                                Orders Orderdata = new Orders();
                                Orderdata.OrderCode = CustomerOrder.OrderCode;
                                Orderdata.NoOfItems = CustomerOrder.NoFoIteams;
                                Orderdata.StoreIdFk = this.Storeid;
                                Orderdata.Amount = CustomerOrder.Amount;
                                Orderdata.CustomerIdFk = CustomerID;
                                Orderdata.EmployeeIdFk = EmployeeID;
                                Orderdata.OrderDate = CustomerOrder.OrderDate;
                                RSCDB.Orders.Add(Orderdata);
                                RSCDB.SaveChanges();
                            }
                        }

                    }
                }
            }
        }
        public bool pushOrderBillingDataToDB(List<CustumerModel> custumerData)
        {
            foreach (var customer in custumerData)
            {
                if (!customer.IsValidCustomer)
                {
                    foreach (var CustomerOrder in customer.custumerOrders)
                    {
                        if (!CustomerOrder.IsValidOrder)
                        {
                            foreach (var OrderBilling in CustomerOrder.OrderBillings)
                            {
                                var OrderID = RSCDB.Orders.Where(o => o.OrderCode == OrderBilling.Ordercode).Select(o => o.OrderIdPk).FirstOrDefault();
                                var BillingData = RSCDB.Billing.FirstOrDefault(B => B.OrderIdFk == OrderID);
                                if (BillingData != null)
                                {
                                    BillingData.Amount = OrderBilling.Amount;
                                    BillingData.BillNumber = OrderBilling.BillingNumber;
                                    BillingData.BillingDate = OrderBilling.BillingDate; 
                                    BillingData.PaymentMode = OrderBilling.ModeOfPayment;
                                    RSCDB.SaveChanges();
                                }
                                else
                                {
                                    Billing Billing = new Billing();
                                    Billing.BillingDate = OrderBilling.BillingDate;
                                    Billing.BillNumber = OrderBilling.BillingNumber;
                                    Billing.OrderIdFk = OrderID;    
                                    Billing.Amount = OrderBilling.Amount;
                                    Billing.PaymentMode = OrderBilling.ModeOfPayment;
                                    RSCDB.Billing.Add(Billing);
                                    RSCDB.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
