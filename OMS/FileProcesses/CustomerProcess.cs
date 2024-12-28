using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FileModel;
using Configuration;
using ProjectHelpers;
using DBDataAcesses;
using OMS_IDAL;
using OMSEntityDAL;
namespace FileProcesses
{
    public class CustomerProcess : BaseProcessor
    {
        private string CustomerFilePath { get; set; }
        public string OrdersFileName { get { return Path.GetFileName(OrdersFilePath); } }
        private string OrdersFilePath { get; set; }
        private string OrderItemFilePath { get; set; }
        private string[] CustomerContent { get; set; }
        private List<CustomerModel> Customers { get; set; }
        private ICustomerDAL objCustomerDal {  get; set; }
        private int WareHouseId { get; set; }
        public CustomerProcess(string CustomerFilePath, string Orderfilepath, string Orderitemfilepath, ICustomerDAL objCustomerDal,int wareHouseid)
        {
            this.CustomerFilePath = CustomerFilePath;
            this.OrdersFilePath = Orderfilepath;
            this.OrderItemFilePath = Orderitemfilepath;
            this.objCustomerDal= objCustomerDal;
            WareHouseId = wareHouseid;
        }

        public void Process()
        {
            if (string.IsNullOrEmpty(this.CustomerFilePath) && string.IsNullOrEmpty(this.OrdersFilePath) && string.IsNullOrEmpty(this.OrderItemFilePath))
            {
                Console.WriteLine("Customer file missing.");
                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Customer file which is  associated with the warehouse id {WareHouseId} is missing.Please check and update the file.\n");
                return;
            }

            ReadFileData();
            ValidateCustomerData();

            bool isSucess=objCustomerDal.PushCustomerDataToDB(Customers, WareHouseId);

            if (isSucess)
            {
                FileHelper.MoiveFile(CustomerFilePath, Enum.FileStatus.Success);
                FileHelper.MoiveFile(OrdersFilePath, Enum.FileStatus.Success);
                FileHelper.MoiveFile(OrderItemFilePath, Enum.FileStatus.Success);
            }
            else
            {
                FileHelper.MoiveFile(CustomerFilePath, Enum.FileStatus.Failure);
                FileHelper.MoiveFile(OrdersFilePath, Enum.FileStatus.Failure);
                FileHelper.MoiveFile(OrderItemFilePath, Enum.FileStatus.Failure);
            }
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
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{orderRow["WareHouseCode"]} has Invalid date.InvoiceNo:{orderRow["InvoiceNo"]}; Please check and Update the file\n");

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
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{orderRow["WareHouseCode"]} has Invalid Price.InvoiceNo:{orderRow["InvoiceNo"]}; Please check and Update the file\n");
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
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{orderRow["WareHouseCode"]} has Invalid Recprd because it doesn't belongs to any customer.InvoiceNo:{orderRow["InvoiceNo"]}; Please check and Update the file\n");
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
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The OrderItem file which is  associated with the warehouse code:{orderItemRow["WareHouseCode"]} has Invalid Quanity.InvoiceNo:{orderItemRow["InvoiceNo"]}; Please check and Update the file\n");
                    }
                    if (decimal.TryParse(Convert.ToString(orderItemRow["PricePerUnit"]), out decimal PricePerUnit))
                    {
                        itemsModel.PricePerUnit = PricePerUnit;
                    }
                    else
                    {
                        itemsModel.IsValidItem = false;
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{orderItemRow["WareHouseCode"]} has Invalid Price.InvoiceNo:{orderItemRow["InvoiceNo"]}; Please check and Update the file\n");
                    }
                    if (decimal.TryParse(Convert.ToString(orderItemRow["TotalAmount"]), out decimal TotalAmount))
                    {
                        itemsModel.TotalAmount = TotalAmount;
                    }
                    else
                    {
                     
                        itemsModel.IsValidItem = false;
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{orderItemRow["WareHouseCode"]} has Invalid Price.InvoiceNo:{orderItemRow["InvoiceNo"]}; Please check and Update the file\n");
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
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{orderItemRow["WareHouseCode"]} is not added to DB it has some invalid values.InvoiceNo:{orderItemRow["InvoiceNo"]}; Please check and Update the file\n");
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
           
                foreach (var CustomerRecord in Customers)
                {

                    if (CustomerRecord.ContactNumber.Length != 10)
                    {
                        CustomerRecord.IsValidCustomer = false;
                        FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Customer file which is  associated with the warehouse code:{CustomerRecord.Orders.Select(x=>x.WareHouseCode).FirstOrDefault()} has invalid customer contactNo and orders of this customer also not affected in DB.CustomerName:{CustomerRecord.CustomerName}; Please check and Update the file\n");
                        continue;
                    }
                    foreach (var OrderRecord in CustomerRecord.Orders)
                    {
                        if (OrderRecord.IsValidOrder == false)
                            continue;
                        if (OrderRecord.InvoiceNumber == string.Empty)
                        {
                            OrderRecord.IsValidOrder = false;

                            FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Order file which is  associated with the warehouse code:{OrderRecord.WareHouseCode} has empty InvoiceNumber, InvoiceNumber is Mandatory.CustomerName:{CustomerRecord.CustomerName}; Please check and Update the file\n");

                            continue;
                        }
                        foreach (var ItemRecord in OrderRecord.Items)
                        {
                            if (!ItemRecord.IsValidItem)
                                continue;
                            if (ItemRecord.InvoiceNumber == string.Empty)
                            {
                                ItemRecord.IsValidItem = false;
                                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The OrderItem file which is  associated with the warehouse code:{OrderRecord.WareHouseCode} has empty InvoiceNumber, InvoiceNumber is Mandatory.CustomerName:{CustomerRecord.CustomerName}; Please check and Update the file\n");
                                continue;
                            }



                        }

                    }

                }

            
           
        }
        




    }

}
