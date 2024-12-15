using System;
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
using FileModel;
using FileProcesses;
using OMS;

namespace FileProcesses
{
    public class CustomerProcess : BaseProcessor
    {
        private string CustomerFilePath { get; set; }
        public string OrdersFileName { get { return Path.GetFileName(OrdersFilePath); } }
        private string OrdersFilePath { get; set; }
        private string OrderItemFilePath { get; set; }
        private string[] CustomerContent { get; set; }
        private int WareHouseId { get; set; }
        private List<CustomerModel> Customers { get; set; }

        public CustomerProcess(string CustomerFilePath, string Orderfilepath, string Orderitemfilepath, int wareHouseId)
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
            ValidateCustomerData();
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


        private void PrepareCustomerModel()
        {
            Customers = new List<CustomerModel>();
            try
            {
                for (int i = 1; i < CustomerContent.Length; i++)
                {
                    var contentarr = CustomerContent[i].Split('|');
                    var customerModel = new CustomerModel();
                    if (contentarr.Length != 2 || contentarr[1].Length!=10)
                    {
                        customerModel.IsValidCustomer = false;
                        continue;
                    }
                    //logic
                    customerModel.CustomerName = contentarr[0];
                    customerModel.ContactNumber = contentarr[1];

                    Customers.Add(customerModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void AssignOrdersToCustomersModel(DataSet dsOrders)
        {
            try
            {
                foreach (DataRow orderRow in dsOrders.Tables[0].Rows)
                {
                    OrdersModel ordersModel = new OrdersModel();
                    //read datarow and fill the model
                    // InvoiceNo          

                    ordersModel.InvoiceNumber = Convert.ToString(orderRow["InvoiceNo"]);
                    ordersModel.WareHouseCode = Convert.ToString(orderRow["WareHouseCode"]);
                    ordersModel.WareHouseIdFk = this.WareHouseId;
                    ordersModel.CustomerPhNo = Convert.ToString(orderRow["CustomerPhNo"]);
                    if (DateTime.TryParse(Convert.ToString(orderRow["Date"]), out DateTime orderDate))
                    {
                        ordersModel.Date = orderDate;
                    }
                    else
                    {
                        ordersModel.IsValidOrder = false;
                        continue;
                    }
                    ordersModel.NoOfItems = Convert.ToInt32(orderRow["NoOfItems"]);
                    ordersModel.PaymentStaus = Convert.ToString(orderRow["PaymentStatus"]);
                    if (decimal.TryParse(Convert.ToString(orderRow["TotalAmount"]), out decimal TotalAmount))
                    {
                        ordersModel.TotalAmount = TotalAmount;
                    }
                    else
                    {
                        ordersModel.IsValidOrder = false;
                        continue;
                    }

                    var customer = Customers.FirstOrDefault(x => x.ContactNumber == ordersModel.CustomerPhNo);

                    if (customer != null)
                    {
                        if (customer.Orders == null)
                        {
                            customer.Orders = new List<OrdersModel>();
                        }

                        customer.Orders.Add(ordersModel);

                    }
                    else
                    {
                        ordersModel.IsValidOrder = false;
                    }

                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void AssignOrderItemsToCustomerOrders(DataSet dsOrderItems)
        {
            try
            {
                foreach (DataRow orderItemRow in dsOrderItems.Tables[0].Rows)
                {
                    bool isAddedToCustomer = false;
                    OrderItemsModel itemsModel = new OrderItemsModel();
                    //InvoiceNo	WareHouseCode			PricePerUnit	TotalAmount
                    itemsModel.InvoiceNumber = Convert.ToString(orderItemRow["InvoiceNo"]);
                    itemsModel.WareHouseCode = Convert.ToString(orderItemRow["WareHouseCode"]);
                    itemsModel.ProductCode = Convert.ToString(orderItemRow["ProductCode"]);
                    itemsModel.ProductCode = Convert.ToString(orderItemRow["ProductCode"]);
                    if (decimal.TryParse(Convert.ToString(orderItemRow["Quanity"]), out decimal Quanity))
                    {
                        itemsModel.Quantity = Quanity;
                    }
                    else
                    {
                        itemsModel.IsValidItem = false;
                    }
                    if (decimal.TryParse(Convert.ToString(orderItemRow["PricePerUnit"]), out decimal PricePerUnit))
                    {
                        itemsModel.PricePerUnit = PricePerUnit;
                    }
                    else
                    {
                        itemsModel.IsValidItem = false;
                    }
                    if (decimal.TryParse(Convert.ToString(orderItemRow["TotalAmount"]), out decimal TotalAmount))
                    {
                        itemsModel.TotalAmount = TotalAmount;
                    }
                    else
                    {
                        itemsModel.IsValidItem = false;
                    }

                    foreach (var customer in Customers)
                    {
                        var order = customer.Orders.FirstOrDefault(x => x.InvoiceNumber == itemsModel.InvoiceNumber);
                        if (order != null)
                        {
                            if (order.Items == null)
                            {
                                order.Items = new List<OrderItemsModel>();
                            }
                            if (itemsModel.IsValidItem == false)
                            {
                                order.IsValidOrder = false;
                            }

                            order.Items.Add(itemsModel);
                            isAddedToCustomer = true;
                            break;
                        }

                    }
                    if (!isAddedToCustomer)
                    {
                        //logger
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        

        

        private void ValidateCustomerData()
        {
            //all validation are done in file reading.

            ValidateOrdersAndOrderItemdataData();
            

        }
        private void ValidateOrdersAndOrderItemdataData()
        {
            try
            {
                foreach (var CustomerRecord in Customers)
                {

                    if (CustomerRecord.ContactNumber.Length != 10)
                    {
                        CustomerRecord.IsValidCustomer = false;
                        continue;
                    }
                    foreach (var OrderRecord in CustomerRecord.Orders)
                    {
                        if (OrderRecord.IsValidOrder == false)
                            continue;
                        if (OrderRecord.InvoiceNumber == string.Empty)
                        {
                            OrderRecord.IsValidOrder = false;
                            continue;
                        }
                        foreach (var ItemRecord in OrderRecord.Items)
                        {
                            if (!ItemRecord.IsValidItem)
                                continue;
                            if (ItemRecord.InvoiceNumber == string.Empty)
                            {
                                ItemRecord.IsValidItem = false;
                                continue;
                            }



                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private void PushCustomerDataToDB()
        {
            try
            { 
                using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {

                        command.CommandText = "UpdateOrInsertCustomer";
                        command.Connection = connection;
                        command.CommandType=CommandType.StoredProcedure;
                        connection.Open();
                        foreach (var customerRecord in Customers)
                        {
                            if (!customerRecord.IsValidCustomer) continue;
                            command.Parameters.Clear();
                            command.Parameters.Add("@CustomerName", DbType.String).Value = customerRecord.CustomerName;
                            command.Parameters.Add("@PhNo", DbType.String).Value = customerRecord.ContactNumber;
                            command.ExecuteNonQuery();

                        }

                        connection.Close();


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            PushOrderDataToDB();

        }
        private void PushOrderDataToDB()
        {
            try 
            { 
                using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "AddOrder";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        foreach (var cust in Customers)
                        {
                            if (!cust.IsValidCustomer) continue;
                            foreach (var OrderRecord in cust.Orders)
                            {
                                if (!OrderRecord.IsValidOrder) continue;

                                command.Parameters.Clear();
                                command.Parameters.Add("@InvoiceNumber", DbType.String).Value = OrderRecord.InvoiceNumber;
                                command.Parameters.Add("@PhNo", DbType.String).Value = OrderRecord.CustomerPhNo;
                                command.Parameters.Add("@WareHouseIdfk", DbType.Int32).Value = WareHouseId;
                                command.Parameters.Add("@OrderDate", DbType.DateTime).Value = OrderRecord.Date;
                                command.Parameters.Add("@NoOfItems", DbType.Int32).Value = OrderRecord.NoOfItems;
                                command.Parameters.Add("@PaymentStatus", DbType.String).Value = OrderRecord.PaymentStaus;
                                command.Parameters.Add("@TotalAmount", DbType.Decimal).Value = OrderRecord.TotalAmount;

                                command.ExecuteNonQuery();


                            }
                        }

                        connection.Close();
                    }

                    PushOrderItemDataToDB();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void PushOrderItemDataToDB()
        {
            try 
            { 
                using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "AddOrderItem";
                        command.Connection = connection;
                        command.CommandType=CommandType.StoredProcedure;
                        connection.Open();
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
                                    command.Parameters.Add("@WareHouseIdfk", DbType.Int32).Value = WareHouseId;              
                                    command.Parameters.Add("@Quantity", DbType.Decimal).Value = OrderItem.Quantity;          
                                    command.Parameters.Add("@PricePerUnit", DbType.Decimal).Value = OrderItem.PricePerUnit;  
                                    command.Parameters.Add("@TotalAmount", DbType.Decimal).Value = OrderItem.TotalAmount;
                                    command.ExecuteNonQuery();

                                }

                            }
                        }

                        connection.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
