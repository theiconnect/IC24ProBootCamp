using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;
using OMS_IDAL;
using OMSEntityDAL.EF;

namespace OMSEntityDAL
{
    public class CustomerEntityDAL: ICustomerDAL
    {

        OMSEntities OMSEntities { get; set; }
        public  CustomerEntityDAL()
        {
                OMSEntities = new OMSEntities();
        }
        public  bool PushCustomerDataToDB(List<CustomerModel> Customers, int wareHouseId)
        {
            try
            {
                foreach (var customerRecord in Customers)
                {
                    if (!customerRecord.IsValidCustomer) continue;

                    var customer =OMSEntities.customers.FirstOrDefault(x=> x.PhNo==customerRecord.ContactNumber);
                    if (customer == null)
                    {
                        customers newcustomer=new customers();
                        newcustomer.CustomerName = customerRecord.CustomerName;
                        newcustomer.PhNo = customerRecord.ContactNumber;
                        OMSEntities.customers.Add(newcustomer);
                        OMSEntities.SaveChanges();

                    }
                    else
                    {
                        customer.CustomerName = customerRecord.CustomerName;
                        OMSEntities.SaveChanges();
                    }

                }


                return PushOrderDataToDB(Customers, wareHouseId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }
        public  bool PushOrderDataToDB(List<CustomerModel> Customers, int wareHouseId)
        {
            try
            {
                foreach (var customerRecord in Customers)
                {
                    if (!customerRecord.IsValidCustomer) continue;
                    foreach (var orderRecord in customerRecord.Orders)
                    {
                        if (!orderRecord.IsValidOrder) continue;
                        var order=OMSEntities.Orders.FirstOrDefault(x=> x.InvoiceNumber==orderRecord.InvoiceNumber);
                        var customerIdFk=OMSEntities.customers.Where(x=>x.PhNo==orderRecord.CustomerPhNo).Select(x=> x.CustomerIdpk).FirstOrDefault();
                        var paymentsStatusIdfk = OMSEntities.PaymentStatus.Where(x => x.PaymentStatus1 == orderRecord.PaymentStaus).Select(x=>x.PaymentStatusIdpk).FirstOrDefault();
                        if (order == null)
                        {
                            Orders orders = new Orders();
                            orders.InvoiceNumber = orderRecord.InvoiceNumber;
                            orders.WareHouseIdfk = wareHouseId;
                            orders.CustomerIdfk = customerIdFk;
                            orders.NoOfItems=orderRecord.NoOfItems;
                            orders.OrderDate= orderRecord.Date;
                            orders.TotalAmount=orderRecord.TotalAmount;
                            orders.PaymentStatusIdfk = paymentsStatusIdfk;
                            OMSEntities.Orders.Add(orders);
                            OMSEntities.SaveChanges();
                        }
                    }
                }

                return PushOrderItemDataToDB(Customers, wareHouseId);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"{ex.Message}");
                return false; 
            }

        }
        public  bool PushOrderItemDataToDB(List<CustomerModel> Customers, int wareHouseId)
        {
            try
            {
                foreach (var customerRecord in Customers)
                {
                    if (!customerRecord.IsValidCustomer) continue;
                    foreach(var orderRecord in customerRecord.Orders)
                    {
                        if (!orderRecord.IsValidOrder) continue;
                        foreach(var orderitemRecord in orderRecord.Items)
                        {
                            if (!orderitemRecord.IsValidItem) continue;
                            var orderIdFk=OMSEntities.Orders.Where(x=> x.InvoiceNumber==orderitemRecord.InvoiceNumber).Select(x=>x.OrderIdpk).FirstOrDefault();
                            var productIdFk = OMSEntities.Products.Where(x => x.ProductCode == orderitemRecord.ProductCode).Select(x => x.ProductIdpk).FirstOrDefault();
                            var paymentsStatusIdfk = OMSEntities.PaymentStatus.Where(x => x.PaymentStatus1 == orderRecord.PaymentStaus).Select(x => x.PaymentStatusIdpk).FirstOrDefault();
                            
                                OrderItem orderItem = new OrderItem();
                                orderItem.OrderIdfk = orderIdFk;
                                orderItem.WarehouseIdfk = wareHouseId;
                                orderItem.ProductIdfk = productIdFk;
                                orderItem.Quantity=orderitemRecord.Quantity;
                                orderItem.PricePerUnit = orderitemRecord.PricePerUnit;
                                orderItem.TotalAmount = orderitemRecord.TotalAmount;
                                OMSEntities.OrderItem.Add(orderItem);
                                OMSEntities.SaveChanges();

                        }

                    }
                }

                return true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine (ex.Message);
                return false;
            }

        }

    }
}
