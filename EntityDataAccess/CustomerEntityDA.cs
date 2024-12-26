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
                var dbCustomer = RSCDB.Customer.FirstOrDefault(c => c.ContactNumber == customer.ContactNumber);

                if (dbCustomer == null)
                {
                    Customer customerObj = new Customer();
                    customerObj.Name = customer.Name;
                    customerObj.ContactNumber = customer.ContactNumber;
                    customerObj.CustomerCode = customer.CustomerCode;
                    customerObj.Email = customer.Email;
                    RSCDB.Customer.Add(customerObj);
                    RSCDB.SaveChanges();
                }
                else
                {
                    Customer customerObj = new Customer();
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

                    var CustomerIdFk = RSCDB.Customer.Where(c => c.CustomerCode == customerOrder.CustomerCode).Select(s => s.CustomerIdPk).FirstOrDefault();

                    var EmployeeIdFK = RSCDB.Employee.Where(E => E.EmployeeCode == customerOrder.EmployeeCode).Select(s => s.EmployeeIdPk).FirstOrDefault();

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
                        var orderIdFk = RSCDB.Orders.Where(o => o.OrderCode == billing.OrderCode).Select(s => s.OrderIdPk).FirstOrDefault();
                        Billing billingObj = new Billing();
                        billingObj.BillingDate = billing.BillingDate;
                        billingObj.BillingNumber = billing.BillingNumber;
                        billingObj.PaymentMode = billing.ModeOfPayment;
                        billingObj.Amount = billing.Amount;
                        billingObj.OrderIdFK = orderIdFk;
                        RSCDB.Billing.Add(billingObj);
                        RSCDB.SaveChanges();
                    }
                }
            }
        }
    }
}

