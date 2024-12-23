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
                var CustomerDt = RSCDB.Customers.FirstOrDefault(c=> c.CustomerCode == Customerdata.CustomerCode && c.ContactNumber == Customerdata.ContactNumber);
                if(CustomerDt != null)
                {
                    CustomerDt.Name = Customerdata.CustomerName;
                    CustomerDt.Email = Customerdata.CustomerEmail;
                    RSCDB.SaveChanges();
                }
                else
                {
                    Customer CustomerData = new Customer();
                    CustomerData.ContactNumber = Customerdata.ContactNumber;
                    CustomerData.CustomerCode = Customerdata.CustomerCode;
                    CustomerData.Email = Customerdata.CustomerEmail;
                    CustomerData.Name = Customerdata.CustomerName;
                    RSCDB.Customers.Add(CustomerData);
                    RSCDB.SaveChanges();    
                }
            }
        }

      
        public void CustomerOrderPushToDB(List<CustumerModel> custumerData)
        {
            foreach(var Customer in custumerData)
            {
                foreach (var CustomerOrder in Customer.custumerOrders)
                {
                    var CustomerID = RSCDB.Customers.Where(c => c.CustomerCode == CustomerOrder.CustomerCode).Select(c => c.CustomerIdPk).FirstOrDefault();
                    var EmployeeID = RSCDB.Employees.Where(E=> E.EmpCode== CustomerOrder.EmployeeCode).Select(E=>E.EmployeeIdPk).FirstOrDefault();
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
                        Order Orderdata = new Order();
                        Orderdata.OrderCode = CustomerOrder.OrderCode;
                        Orderdata.NoOfItems = CustomerOrder.NoFoIteams;
                        Orderdata.StoreIdFk = this.Storeid;
                        Orderdata.Amount = CustomerOrder.Amount;
                        Orderdata.CustomerIdFk = CustomerID;
                        Orderdata.EmployeeIdFk= EmployeeID;
                        Orderdata.OrderDate = CustomerOrder.OrderDate;
                        RSCDB.Orders.Add(Orderdata);
                        RSCDB.SaveChanges();
                    }

                }
            }
        }
        public bool pushOrderBillingDataToDB(List<CustumerModel> custumerData)
        {
            
            return true;
        }
    }
}
