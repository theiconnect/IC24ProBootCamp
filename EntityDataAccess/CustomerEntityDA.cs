using EntityDataAccess.EF;
using IDataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDataAccess
{
    public class CustomerEntityDA : ICustomerDA
    {
        RscEntities RSCDB { get; set; }
        public CustomerEntityDA()
        {
            RSCDB = new RscEntities();
        }
        public bool SyncCustomerOrderData(List<CustomerModel> customers)
        {
            foreach (var customer in customers)
            {
                var dbCustomer = RSCDB.Customers.FirstOrDefault(c => c.ContactNumber == customer.ContactNumber);

                if (dbCustomer == null)
                {
                    Customers customerObj = new Customers();
                    customerObj.Name = customer.Name;
                    customerObj.ContactNumber = customer.ContactNumber;
                    customerObj.CustomerCode = customer.CustomerCode;
                    customerObj.Email = customer.Email;
                    RSCDB.Customers.Add(customerObj);
                    RSCDB.SaveChanges();
                }
                else
                {
                    Customers customerObj = new Customers();
                    customerObj.ContactNumber = customer.ContactNumber;
                    customerObj.Email = customer.Email;
                    RSCDB.SaveChanges();
                }
            }
            SyncCustomerOrderDataWithDB(customers);
            SyncCustomerOrderBilling(customers);
            return true;
        }
        public void SyncCustomerOrderDataWithDB(List<CustomerModel> customers)
        {
            foreach (var customer in customers)
            {
                foreach (var customerOrder in customer.CustomerOrders)
                {
                    var storeIdFK = RSCDB.Stores.Where(s => s.StoreCode == customerOrder.StoreCode).Select(s => s.StoreIdPk).FirstOrDefault();

                    var CustomerIdFk = RSCDB.Customers.Where(c => c.CustomerCode == customerOrder.CustomerCode).Select(s => s.CustomerIdPk).FirstOrDefault();

                    var EmployeeIdFK = RSCDB.Employee.Where(E => E.EmployeeCode == customerOrder.EmployeeCode).Select(s => s.EmployeeIdPk).FirstOrDefault();
                    var dbCustomerOrder = RSCDB.Orders.FirstOrDefault(o => o.CustomerIdFk == CustomerIdFk);



                    if (dbCustomerOrder == null)
                    {
                        Orders orderObj = new Orders();
                        orderObj.StoreIdFk = storeIdFK;
                        orderObj.CustomerIdFk = CustomerIdFk;
                        orderObj.EmployeeIdFk = EmployeeIdFK;
                        orderObj.OrderDate = customerOrder.OrderDate;
                        orderObj.OrderCode = customerOrder.OrderCode;
                        orderObj.NoOfItems = customerOrder.NoOfItems;
                        orderObj.TotalAmount = customerOrder.Amount;
                        RSCDB.Orders.Add(orderObj);
                        RSCDB.SaveChanges();

                    }
                    else
                    {
                        ///Orders orderObj = new Orders();
                        dbCustomerOrder.StoreIdFk = storeIdFK;
                        dbCustomerOrder.CustomerIdFk = CustomerIdFk;
                        dbCustomerOrder.EmployeeIdFk = EmployeeIdFK;
                        dbCustomerOrder.OrderDate = customerOrder.OrderDate;
                        dbCustomerOrder.OrderCode = customerOrder.OrderCode;
                        dbCustomerOrder.NoOfItems = customerOrder.NoOfItems;
                        dbCustomerOrder.TotalAmount = customerOrder.Amount;
                        RSCDB.SaveChanges();

                    }


                }
            }
        }


        public void SyncCustomerOrderBilling(List<CustomerModel> customers)
        {
            foreach (var customer in customers)
            {
                foreach (var orders in customer.CustomerOrders)
                {
                    foreach (var billing in orders.OrderBilling)
                    {
                        var orderId = RSCDB.Orders.Where(o => o.OrderCode == billing.OrderCode).Select(s => s.OrderIdPk).FirstOrDefault();
                        var dbBilling = RSCDB.Billing.FirstOrDefault(b => b.OrderIdFk == orderId);
                        // var dbCustomerBilling = RSCDB.Billing.FirstOrDefault(b => b.BillNumber == billing.BillingNumber);
                        //if()
                        if (dbBilling == null)
                        {
                            Billing billingObj = new Billing();
                            billingObj.BillingDate = billing.BillingDate;
                            billingObj.BillNumber = billing.BillingNumber;
                            billingObj.PaymentMode = billing.ModeOfPayment;
                            billingObj.Amount = billing.Amount;
                            billingObj.OrderIdFk = orderId;
                            RSCDB.Billing.Add(billingObj);
                            RSCDB.SaveChanges();

                        }
                        else
                        {
                            dbBilling.BillingDate = billing.BillingDate;
                            dbBilling.BillNumber = billing.BillingNumber;
                            dbBilling.PaymentMode = billing.ModeOfPayment;
                            dbBilling.Amount = billing.Amount;
                            dbBilling.OrderIdFk = orderId;
                            
                            RSCDB.SaveChanges();

                        }

                        
                        
                    }
                }
            }
        }
    }
}

