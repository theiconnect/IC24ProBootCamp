using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_arjun.Models;
using OMS_arjun;

namespace OMS_Arjun_v2
{
    internal class CustomerProcess:BaseProcessor
    {
        private string CustomerFilePath { get; set; }
        public string OrdersFileName { get { return Path.GetFileName(OrdersFilePath); } }
        private string OrdersFilePath { get; set; }
        private string OrderItemFilePath { get; set; }
        private string[] CustomerContent { get; set; }
        private string OrdersContent { get; set; }
        private int WareHouseId { get; set; }

        private List<CustomerModel> Customers { get; set; }



        public CustomerProcess(string CustomerFilePath, string Orderfilepath, string Orderitemfilepath, int wareHouseId)
        {
            this.CustomerFilePath = CustomerFilePath;
            this.OrdersFilePath = Orderfilepath;
            this.OrderItemFilePath = Orderitemfilepath;
            this.WareHouseId = wareHouseId;
        }

        internal void Process()
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
            BulidRelationshipBetweenFiles();
        }

        private void BulidRelationshipBetweenFiles()
        {


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
                foreach (var customer in Customers)
                {
                    var order = customer.Orders.FirstOrDefault(x => x.InvoiceNumber == itemsModel.InvoiceNumber);
                    if (order != null)
                    {
                        if (order.Items == null)
                        {
                            order.Items = new List<OrderItemsModel>();
                        }
                        order.Items.Add(itemsModel);
                        itemsModel.IsValidRecord = true;
                        break;

                    }
                    else
                    {
                        itemsModel.IsValidRecord = false;
                    }

                }




            }
        }

        private void AssignOrdersToCustomersModel(DataSet dsOrders)
        {

            foreach (DataRow orderRow in dsOrders.Tables[0].Rows)
            {
                OrdersModel ordersModel = new OrdersModel();
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

                var customer = Customers.FirstOrDefault(x => x.PhNo == ordersModel.CustomerPhNo);

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
                customerModel.CustomerName =Convert.ToInt32(contentarr[0]);
                customerModel.PhNo = contentarr[1];
                Customers.Add(customerModel);
            }
        }

        private void ValidateOrderData()
        {


        }

        private void PushOrderDataToDB()
        {
            using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "in";
                }
            }
        }
        private void PushCustomerDataToDB()
        {
            using (SqlConnection connection = new SqlConnection(DBHelper.oMSConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {

                    command.CommandText = "if exists (select PhNo from Customers where PhNo=@PhNo)\r\nbegin\r\nUpdate Customers set CustomerName=@CustomerName\r\nend\r\nelse\r\nbegin\r\nInsert InTo Customers(CustomerName,PhNo)\r\nvalues(@CustomerName,@PhNO)\r\nend";
                    command.Connection = connection;
                    connection.Open();
                    foreach (var customerRecord in Customers)
                    {
                        command.Parameters.Add("@CustomerName", DbType.String).Value = customerRecord.CustomerName;
                        command.Parameters.Add("@PhNo", DbType.String).Value = customerRecord.PhNo;
                        command.ExecuteNonQuery();

                    }

                    connection.Close();


                }
            }
            PushOrderDataToDB();

        }
    }
}
