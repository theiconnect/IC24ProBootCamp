using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Models;
using System.Reflection;
using System.Text.RegularExpressions;
using DataAccess;
using PathAndDataBaseConfig;
using IDataAccess;



namespace BusinessAccessLayer
{
    public class CustomerProcess
    {
        private string CustomerFilePath { get; set; }
        private int StoreIdFk { get; set; }
        private List<CustomerModel> customers { get; set; }
        private CustomerOrderModel orderModel { get; set; }
        private OrderBillingModel billingModel { get; set; }
        private bool isValidFile { get; set; }
        private string FailReason { get; set; }
        private DataSet ds { get; set; }
        private ICustomerDA objCustomerDA {  get; set; }
        
        

        public CustomerProcess(string customerFilePath, int storeIdFk, ICustomerDA objICustomerDA)
        {
            this.objCustomerDA = objICustomerDA;
            this.CustomerFilePath = customerFilePath;
            this.StoreIdFk = storeIdFk;

        }
        public void Process()
        {
            ReadCustomerData("Customer", "CustomerOrders", "CustomerBilling");
            PrepareCustomerModel();
            ValidateCustomerData();
            PushCustomerDataToDB();   
        }
        //IMEX=1 ensures that mixed data types (e.g., both numbers and text in the same column) are handled as text,
        //preventing errors and inconsistencies.
        public void ReadCustomerData(params string[] sheets)
        {
            ds = new DataSet();
            string conn = string.Format(BaseProcessor.excelConnectionString, CustomerFilePath);
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                con.Open();
                foreach (var sheet in sheets)
                {
                    // Create a new DataTable for each sheet
                    DataTable dt = new DataTable(sheet);
                    using (OleDbDataAdapter da = new OleDbDataAdapter($"select * from  [{sheet}$]", con))
                    {
                        da.Fill(dt);
                    }
                    RemoveEmptyRows(dt);

                    // Add the filled DataTable to the DataSet
                    ds.Tables.Add(dt);
                }
                DataTable customerTable = ds.Tables["Customer"];
                DataTable orderTable = ds.Tables["CustomerOrders"];
                DataTable billingTable = ds.Tables["CustomerBilling"];
                con.Close();
            }
            
        }
        private void RemoveEmptyRows(DataTable dt)
        {
            // We iterate backwards to avoid issues while removing rows
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                bool isEmptyRow = true;

                // Check if all columns in this row are DBNull or null
                foreach (DataColumn column in dt.Columns)  // Explicitly type the column as DataColumn
                {
                    if (dt.Rows[i][column] != DBNull.Value && dt.Rows[i][column] != null)
                    {
                        isEmptyRow = false;
                        break;
                    }
                }

                // If this row is empty, remove it from the DataTable
                if (isEmptyRow)
                {
                    dt.Rows.RemoveAt(i);
                }
            }
        }
        private void PrepareCustomerModel()
        {
            DataTable customertable = ds.Tables[0];
            DataTable ordertable = ds.Tables[1];
            DataTable billingtable = ds.Tables[2];
            customers = new List<CustomerModel>();
            foreach (DataRow customer in customertable.Rows)
            {
                CustomerModel customerModel = new CustomerModel();
                customerModel.IsValidCustomer = true;
                customerModel.CustomerCode = Convert.ToString(customer["CustomerCode"]);
                customerModel.Name = Convert.ToString(customer["Name"]);
                customerModel.Email = Convert.ToString(customer["Email"]);
                customerModel.ContactNumber = Convert.ToString(customer["ContactNumber"]);
                this.customers.Add(customerModel);
            }
            
            foreach (DataRow orders in ordertable.Rows)
            {
                orderModel = new CustomerOrderModel();
                orderModel.IsValidOrder = true;
                if (orders["CustomerCode"] != DBNull.Value)
                    orderModel.CustomerCode = Convert.ToString(orders["CustomerCode"]);

                orderModel.OrderCode = Convert.ToString(orders["OrderCode"]);
                if (orders["StoreCode"] != DBNull.Value)
                    orderModel.StoreCode = Convert.ToString(orders["StoreCode"]);
                if (orders["EmployeeCode"] != DBNull.Value)
                    orderModel.EmployeeCode = Convert.ToString(orders["EmployeeCode"]);

                orderModel.ProductCode = Convert.ToString(orders["ProductCode"]);
                if (DateTime.TryParse(Convert.ToString(orders["OrderDate"]), out DateTime orderDate))
                    orderModel.OrderDate = orderDate;
                else
                {
                    orderModel.IsValidOrder = false;
                }
                if (orders["NoOfItems"] != DBNull.Value)
                    orderModel.NoOfItems = Convert.ToInt32(orders["NoOfItems"]);
                if (decimal.TryParse(Convert.ToString(orders["Amount"]), out decimal amount))
                    orderModel.Amount = amount;
                else
                    orderModel.IsValidOrder = false;

                var customer= customers.FirstOrDefault(x=>x.CustomerCode==orderModel.CustomerCode);
                if(customer!= null)
                {
                    if (customer.CustomerOrders == null)
                    {
                        customer.CustomerOrders = new List<CustomerOrderModel>();
                    }
                    customer.CustomerOrders.Add(orderModel);
                }
                else
                {
                    orderModel.IsValidOrder = false;
                }

            }

            foreach (DataRow billing in billingtable.Rows)
            {
                billingModel = new OrderBillingModel();
                billingModel.IsValidBilling = true;

                billingModel.BillingNumber = Convert.ToString(billing[0]);
                billingModel.ModeOfPayment = Convert.ToString(billing[1]);
                billingModel.OrderCode = Convert.ToString(billing[2]);
                if (billing["BillingDate"] != DBNull.Value)
                    billingModel.BillingDate = Convert.ToDateTime(billing[3]);


                if (billing["Amount "] != DBNull.Value)
                    billingModel.Amount = Convert.ToDecimal(billing[4]);

                foreach (var cust in customers) 
                {
                    var order = cust.CustomerOrders.FirstOrDefault(x => x.OrderCode == billingModel.OrderCode);
                    if (order != null)
                    {
                        if (order.OrderBilling == null)
                        {
                            order.OrderBilling = new List<OrderBillingModel>();
                        }
                        order.OrderBilling.Add(billingModel);
                        break;
                    }
                    else
                    {
                        billingModel.IsValidBilling = false;

                    }
                    
                  

                }

            }





        }
        private void ValidateCustomerData()
        {

            if (customers.Count == 0)
            {
                //No customer is their
                FailReason = "Log the error: No customers found in the file.";
                isValidFile = false;
                return;
            }
            foreach (var customer in customers)
            {
                if (customer.ContactNumber == string.Empty || customer.ContactNumber.Length < 10)
                {
                    customer.IsValidCustomer = false;
                    continue;
                }
                if (customer.Email != string.Empty)
                {
                    bool isEmail = Regex.IsMatch(customer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                    if (isEmail)
                    {
                        customer.IsValidCustomer = false;
                        continue;
                    }
                }

                foreach (var order in customer.CustomerOrders)
                {
                    if (order.IsValidOrder == false)
                        continue;

                    if (order.OrderCode == string.Empty)
                    {
                        order.IsValidOrder = false;
                        continue;
                    }

                    foreach (var billing in order.OrderBilling)
                    {
                        if (billing.BillingNumber == string.Empty)
                        {
                            billing.IsValidBilling = false;
                            order.IsValidOrder = false;
                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                //I am cheking any FailReason is their or not and failreason is their means invalid file
                isValidFile = false;
                return;
            }
            isValidFile = true;
        }
        private void PushCustomerDataToDB()
        {
            bool IsSuccess = objCustomerDA.SyncCustomerOrderData(customers);
            if (IsSuccess)
            {
                FileHelper.Move(CustomerFilePath, FileStatus.Sucess);
            }
            else
            {
                FileHelper.Move(CustomerFilePath, FileStatus.Failure);
            }
        }
    }
}



