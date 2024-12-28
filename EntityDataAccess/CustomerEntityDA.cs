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
        //public void SyncCustomerOrderDataWithDB(List<CustomerModel> customers)
        //{
        //    foreach (var customer in customers)
        //    {
        //        foreach (var customerOrder in customer.CustomerOrders)
        //        {
        //            var storeIdFK = RSCDB.Stores.Where(s => s.StoreCode == customerOrder.StoreCode).Select(s => s.StoreIdPk).FirstOrDefault();

        //            var CustomerIdFk = RSCDB.Customer.Where(c => c.CustomerCode == customerOrder.CustomerCode).Select(s => s.CustomerIdPk).FirstOrDefault();

        //            var EmployeeIdFK = RSCDB.Employee.Where(E => E.EmployeeCode == customerOrder.EmployeeCode).Select(s => s.EmployeeIdPk).FirstOrDefault();
        //            var dbCustomerOrder=RSCDB.Orders.FirstOrDefault(o=>o.OrderCode== customerOrder.OrderCode);
        //            if (dbCustomerOrder == null)
        //            {
        //                Orders orderObj = new Orders();
        //                orderObj.StoreIdFk = storeIdFK;
        //                orderObj.CustomerIdFk = CustomerIdFk;
        //                orderObj.EmployeeIdFk = EmployeeIdFK;
        //                orderObj.OrderDate = customerOrder.OrderDate;
        //                orderObj.OrderCode = customerOrder.OrderCode;
        //                orderObj.NoOfItems = customerOrder.NoOfItems;
        //                orderObj.TotalAmount = customerOrder.Amount;
        //                RSCDB.Orders.Add(orderObj);
        //                RSCDB.SaveChanges();

        //            }
        //            else
        //            {
        //                ///Orders orderObj = new Orders();
        //                dbCustomerOrder.StoreIdFk = storeIdFK;
        //                dbCustomerOrder.CustomerIdFk = CustomerIdFk;
        //                dbCustomerOrder.EmployeeIdFk = EmployeeIdFK;
        //                dbCustomerOrder.OrderDate = customerOrder.OrderDate;
        //                dbCustomerOrder.OrderCode = customerOrder.OrderCode;
        //                dbCustomerOrder.NoOfItems = customerOrder.NoOfItems;
        //                dbCustomerOrder.TotalAmount = customerOrder.Amount;
        //                RSCDB.SaveChanges();

        //            }


        //        }
        //    }
        //}
        public void SyncCustomerOrderDataWithDB(List<CustomerModel> customers)
        {
            foreach (var customer in customers)
            {
                foreach (var customerOrder in customer.CustomerOrders)
                {
                    // Get the foreign key for store
                    var storeIdFK = RSCDB.Stores
                                          .Where(s => s.StoreCode == customerOrder.StoreCode)
                                          .Select(s => s.StoreIdPk)
                                          .FirstOrDefault();

                    // Get the foreign key for customer
                    var customerIdFK = RSCDB.Customer
                                            .Where(c => c.CustomerCode == customerOrder.CustomerCode)
                                            .Select(c => c.CustomerIdPk)
                                            .FirstOrDefault();

                    // Get the foreign key for employee
                    var employeeIdFK = RSCDB.Employee
                                             .Where(e => e.EmployeeCode == customerOrder.EmployeeCode)
                                             .Select(e => e.EmployeeIdPk)
                                             .FirstOrDefault();

                    // Log the results to check what we're getting from the database
                    Console.WriteLine($"StoreIdFK: {storeIdFK}, CustomerIdFK: {customerIdFK}, EmployeeIdFK: {employeeIdFK}");

                    // Check if any of the foreign keys are invalid (e.g., 0 or null)
                    if (storeIdFK == 0 || customerIdFK == 0 || employeeIdFK == 0)
                    {
                        if (storeIdFK == 0)
                            Console.WriteLine($"Store with code {customerOrder.StoreCode} does not exist.");
                        if (customerIdFK == 0)
                            Console.WriteLine($"Customer with code {customerOrder.CustomerCode} does not exist.");
                        if (employeeIdFK == 0)
                            Console.WriteLine($"Employee with code {customerOrder.EmployeeCode} does not exist.");

                        continue; // Skip this order since one or more FK values are invalid
                    }

                    // Try to find the order by OrderCode
                    var dbCustomerOrder = RSCDB.Orders.FirstOrDefault(o => o.OrderCode == customerOrder.OrderCode);

                    if (dbCustomerOrder == null)
                    {
                        // Create and add new order if not found
                        Orders newOrder = new Orders
                        {
                            StoreIdFk = storeIdFK,
                            CustomerIdFk = customerIdFK,
                            EmployeeIdFk = employeeIdFK,
                            OrderDate = customerOrder.OrderDate,
                            OrderCode = customerOrder.OrderCode,
                            NoOfItems = customerOrder.NoOfItems,
                            TotalAmount = customerOrder.Amount
                        };

                        RSCDB.Orders.Add(newOrder);
                    }
                    else
                    {
                        // Update the existing order if found
                        dbCustomerOrder.StoreIdFk = storeIdFK;
                        dbCustomerOrder.CustomerIdFk = customerIdFK;
                        dbCustomerOrder.EmployeeIdFk = employeeIdFK;
                        dbCustomerOrder.OrderDate = customerOrder.OrderDate;
                        dbCustomerOrder.NoOfItems = customerOrder.NoOfItems;
                        dbCustomerOrder.TotalAmount = customerOrder.Amount;
                    }

                    // Save changes to the database
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
                       // var dbCustomerBilling = RSCDB.Billing.FirstOrDefault(b => b.BillNumber == billing.BillingNumber);
                        //if()
                        Billing billingObj = new Billing();
                        billingObj.BillingDate = billing.BillingDate;
                        billingObj.BillNumber = billing.BillingNumber;
                        billingObj.PaymentMode = billing.ModeOfPayment;
                        billingObj.Amount = billing.Amount;
                        billingObj.OrderIdFk = orderIdFk;
                        RSCDB.Billing.Add(billingObj);
                        RSCDB.SaveChanges();
                    }
                }
            }
        }
    }
}

