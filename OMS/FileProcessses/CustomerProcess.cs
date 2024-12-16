﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using OMS;
using Configuration;
using FileModel;
using ProjectHelper;
namespace FileProcessses
{
    public class CustomerProcess: BaseProcessor
    {
        private string CustomerFilePath {  get; set; }
        public string OrdersFileName { get { return Path.GetFileName(OrdersFilePath); }  }
        private string OrdersFilePath { get; set; }
        private string OrderItemFilePath {  get; set; }
        private string[] CustomerContent {  get; set; }
        private string OrdersContent {  get; set; }
        private int WareHouseId { get;set; }

        private List<CustomerModel> Customers {  get; set; }
       


        public CustomerProcess( string CustomerFilePath, string Orderfilepath, string Orderitemfilepath, int wareHouseId) 
        {
            this.CustomerFilePath = CustomerFilePath;
            this.OrdersFilePath = Orderfilepath;
            this.OrderItemFilePath = Orderitemfilepath;
            this.WareHouseId = wareHouseId;
        }

        public void Process()
        {
            if (string.IsNullOrEmpty(this.CustomerFilePath) && string.IsNullOrEmpty(this.OrdersFilePath) && string.IsNullOrEmpty(this.OrderItemFilePath))
            {
                Console.WriteLine("Customer file missing.");
                return;
            }

            ReadFileData();
            ValidateOrderData();
            
            PushCustomerDataToDB();
            

        }

        private void ReadFileData()
        {
            CustomerContent = FileHelper.GetAllLinesOfFlatFileByPath(CustomerFilePath);
            DataSet dsOrders = FileHelper.GetExcelFileContent(OrdersFilePath, "Orders");
            DataSet dsOrderItems = FileHelper.GetExcelFileContent(OrderItemFilePath, "Orderitems");

            if (dsOrders.Tables.Count < 1 || dsOrderItems.Tables.Count < 1 || dsOrders.Tables[0].Rows.Count < 1
                || dsOrderItems.Tables[0].Rows.Count < 1)
            {
                Console.WriteLine("Invalid files");
                return;
            }

            PrepareCustomerModel();
            AssignOrdersToCustomersModel(dsOrders);
            AssignOrderItemsToCustomerOrders(dsOrderItems);
        }

      
        
        private void AssignOrderItemsToCustomerOrders(DataSet dsOrderItems)
        {
            foreach (DataRow orderItemRow in dsOrderItems.Tables[0].Rows)
            {
                OrderItemsModel itemsModel = new OrderItemsModel();
                //InvoiceNo	WareHouseCode			PricePerUnit	TotalAmount
                itemsModel.InvoiceNumber = Convert.ToString(orderItemRow["InvoiceNo"]);
                itemsModel.WareHouseCode = Convert.ToString(orderItemRow["WareHouseCode"]);
                itemsModel.ProductCode = Convert.ToString(orderItemRow["ProductCode"]);
                itemsModel.ProductCode = Convert.ToString(orderItemRow["ProductCode"]);
                if (decimal.TryParse(Convert.ToString(orderItemRow["Quanity"]), out decimal Quanity))
                    itemsModel.Quantity = Quanity;
                if (decimal.TryParse(Convert.ToString(orderItemRow["PricePerUnit"]), out decimal PricePerUnit))
                    itemsModel.PricePerUnit = PricePerUnit;
                if (decimal.TryParse(Convert.ToString(orderItemRow["TotalAmount"]), out decimal TotalAmount))
                    itemsModel.TotalAmount = TotalAmount;
                foreach(var customer in Customers) 
                {
                    var order=customer.Orders.FirstOrDefault(x => x.InvoiceNumber == itemsModel.InvoiceNumber);
                    if (order != null)
                    {
                        if(order.Items == null)
                        {
                            order.Items = new List<OrderItemsModel>();
                        }
                        order.Items.Add(itemsModel);
                        itemsModel.IsValidRecord = true;
                        break;

                    }
                    else
                    {
                        itemsModel.IsValidRecord= false;    
                    }
                    
                }

                


            }
        }

        private void AssignOrdersToCustomersModel(DataSet dsOrders)
        {

            foreach (DataRow orderRow in dsOrders.Tables[0].Rows)
            {
                OrdersModel  ordersModel = new OrdersModel();
                //read datarow and fill the model
               // InvoiceNo          

                ordersModel.InvoiceNumber = Convert.ToString(orderRow["InvoiceNo"]);
                ordersModel.WareHouseCode = Convert.ToString(orderRow["WareHouseCode"]);
                ordersModel.WareHouseIdFk = this.WareHouseId;
                ordersModel.CustomerPhNo = Convert.ToString(orderRow["InvoiCustomerPhNoceNo"]);
                if (DateTime.TryParse(Convert.ToString(orderRow["Date"]), out DateTime orderDate))
                    ordersModel.Date = orderDate;
                ordersModel.PaymentStaus = Convert.ToString(orderRow["PaymentStatus"]);
                if (decimal.TryParse(Convert.ToString(orderRow["TotalAmount"]), out decimal TotalAmount))
                    ordersModel.TotalAmount = TotalAmount;


                var customer=Customers.FirstOrDefault(x => x.ContactNumber == ordersModel.CustomerPhNo);

                if(customer != null)
                {
                    if (customer.Orders == null)
                    { 

                        customer.Orders = new List<OrdersModel>();
                    }
                   
                    customer.Orders.Add(ordersModel);

                }
                else
                {
                    ordersModel.IsValidRecord = false;
                }

            }
        }

        private void PrepareCustomerModel()
        {
            Customers = new List<CustomerModel>();
            for (int i = 1; i < CustomerContent.Length; i++)
            {
                var contentarr = CustomerContent[i].Split('|');
                CustomerModel customerModel = new CustomerModel();
                //logic
                customerModel.CustomerName = contentarr[0];
                customerModel.ContactNumber = contentarr[1];
                Customers.Add(customerModel);
            }
        }

        private void ValidateOrderData()
        {


        }
        private void PushOrderItemDataToDB()
        {
            using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "insert into OrderItems(InvoiceNumber,WareHouseIdfk,CustomerIdfk,OrderDate,NoOfItems,PaymentStatusIdfk,TotalAmount)\r\nvalues(@InvoiceNumber,@WareHouseIdfk,(Select customeridpk from cusrtomers where phno=@Phno),@OrderDate,@NoOfItems,@PaymentStatusIdfk,@TotalAmount)";
                    command.Connection = connection;
                    connection.Open();
                    foreach(var cust in Customers)
                    {
                        foreach(var order in cust.Orders)
                        {
                            command.Parameters.Add("@InvoiceNumber", DbType.String).Value = OrderRecord.InvoiceNumber;
                            command.Parameters.Add("@Phno", DbType.Int16).Value = OrderRecord.CustomerPhNo;
                            command.Parameters.Add("@WareHouseIdfk", DbType.Int16).Value = WareHouseId;
                            command.Parameters.Add("@OrderDate", DbType.DateTime).Value = OrderRecord.Date;
                            command.Parameters.Add("@NoOfItems", DbType.Decimal).Value = OrderRecord.NoOfItems;
                            command.Parameters.Add("@PaymentStatusIdfk", DbType.Int16).Value = OrderRecord.PaymentStaus;
                            command.ExecuteNonQuery();

                        }
                    }
                    
                    connection.Close();
                }

            }
        }
        private void PushOrderDataToDB()
        {
            using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "insert into Orders(InvoiceNumber,WareHouseIdfk,CustomerIdfk,OrderDate,NoOfItems,PaymentStatusIdfk,TotalAmount)\r\nvalues(@InvoiceNumber,@WareHouseIdfk,(Select customeridpk from cusrtomers where phno=@Phno),@OrderDate,@NoOfItems,@PaymentStatusIdfk,@TotalAmount)";
                    command.Connection = connection;
                    connection.Open();
                    
                    foreach(var OrderRecord in Customers.o)
                    {
                        command.Parameters.Add("@InvoiceNumber", DbType.String).Value = OrderRecord.InvoiceNumber;
                        command.Parameters.Add("@Phno", DbType.Int16).Value = OrderRecord.CustomerPhNo;
                        command.Parameters.Add("@WareHouseIdfk", DbType.Int16).Value = WareHouseId;
                        command.Parameters.Add("@OrderDate", DbType.DateTime).Value = OrderRecord.Date;
                        command.Parameters.Add("@NoOfItems", DbType.Decimal).Value = OrderRecord.NoOfItems;
                        command.Parameters.Add("@PaymentStatusIdfk", DbType.Int16).Value = OrderRecord.PaymentStaus;
                        command.ExecuteNonQuery();

                    }
                    connection.Close();
                }

                PushOrderItemDataToDB();
            }
        }
        private void PushCustomerDataToDB()
        {
            using(SqlConnection connection=new SqlConnection(DBHelper.oMSConnectionString))
            {
                using (SqlCommand command=new SqlCommand())
                {
                    
                    command.CommandText = "if exists (select PhNo from Customers where PhNo=@PhNo)\r\nbegin\r\nUpdate Customers set CustomerName=@CustomerName\r\nend\r\nelse\r\nbegin\r\nInsert InTo Customers(CustomerName,PhNo)\r\nvalues(@CustomerName,@PhNO)\r\nend";
                    command.Connection= connection;
                    connection.Open();
                    foreach(var customerRecord in Customers)
                    {
                        command.Parameters.Add("@CustomerName", DbType.String).Value =customerRecord.CustomerName;
                        command.Parameters.Add("@PhNo",DbType.String).Value=customerRecord.ContactNumber;
                        command.ExecuteNonQuery();

                    }
                    
                    connection.Close();
                    

                }
            }
            PushOrderDataToDB();

        }
    }

}
