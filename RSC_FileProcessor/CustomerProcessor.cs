﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RSC_Models;
using RSC_Configurations;
using RSC_DataAccess;

namespace RSC_FileProcessor
{
    public class CustumerProcessor 
    {
        private string customerFilePath { get; set; }
        private int Storeid { get; set; }
        private static DataSet dataSet { get; set; }
        private string FailReason { get; set; }
        private bool isValidFile { get; set; }

        public List<CustumerModel> custumerData { get; set; }


        public CustumerProcessor(string custumerfile, int storeid)
        {
            customerFilePath = custumerfile;
            Storeid = storeid;
        }
        public void processor()
        {
            ReadCustumerData();
            ValidateCustomerData();
            PrepareCustomerData();
            CustomerDBAccess obj = new CustomerDBAccess(custumerData);
            obj.pushOrderBillingDataToDB(custumerData);   
            obj.CustomerOrderPushToDB(custumerData, Storeid);    
        }

        private void ReadCustumerData()
        {
            dataSet = new DataSet();
            using (OleDbConnection connection = new OleDbConnection(AppConfiguration.XlsxConnectionstring))
            {
                try
                {
                    connection.Open();
                    OleDbDataAdapter customerDataAdapter = new OleDbDataAdapter("SELECT * FROM [Customer$]", connection);
                    OleDbDataAdapter customerOrderDataAdapter = new OleDbDataAdapter("SELECT * FROM [CustomerOrders$]", connection);
                    OleDbDataAdapter customerBillingDataAdapter = new OleDbDataAdapter("SELECT * FROM [CustomerBilling$]", connection);
                    customerDataAdapter.Fill(dataSet, "Customer");
                    customerOrderDataAdapter.Fill(dataSet, "CustomerOrders");
                    customerBillingDataAdapter.Fill(dataSet, "CustomerBilling");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
        private void ValidateCustomerData()
        {
            if (custumerData.Count == 0)
            {
                FailReason = "invalid file customer file";
                isValidFile = false;
                return;

            }
            foreach (var customer in custumerData)
            {

                if (customer.ContactNumber == string.Empty || customer.ContactNumber.Length < 10)
                {
                    customer.IsValidCustomer = false;
                    continue;
                }
                if (customer.CustomerEmail != string.Empty)
                {
                    bool isEmail = Regex.IsMatch(customer.CustomerEmail, @"\A(?:[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                    if (isEmail)
                    {
                        customer.IsValidCustomer = false;
                        continue;
                    }
                }

                foreach (var order in customer.custumerOrders)
                {
                    if (order.IsValidOrder == false)
                        continue;

                    if (order.OrderCode == string.Empty)
                    {
                        order.IsValidOrder = false;
                        continue;
                    }

                    foreach (var billing in order.OrderBillings)
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
        private void PrepareCustomerData()
        {
            DataTable dtCustomer = dataSet.Tables[0];
            DataTable dtOrder = dataSet.Tables[1];
            DataTable dtBilling = dataSet.Tables[2];
            custumerData = new List<CustumerModel>();
            foreach (DataRow CustomerTable in dtCustomer.Rows)
            {
                CustumerModel Customermodel = new CustumerModel();
                Customermodel.CustomerCode = CustomerTable[0].ToString();
                Customermodel.CustomerName = CustomerTable[1].ToString();
                Customermodel.CustomerEmail = CustomerTable[2].ToString();
                Customermodel.ContactNumber = CustomerTable[3].ToString();
                custumerData.Add(Customermodel);
                Customermodel.custumerOrders = new List<CustumerOrders>();
                foreach (DataRow OrderTable in dtOrder.Rows)
                {
                    CustumerOrders CustomerOrder = new CustumerOrders();
                    CustomerOrder.OrderCode = OrderTable[0].ToString();
                    CustomerOrder.CustomerCode = OrderTable[1].ToString();
                    CustomerOrder.StoreCode = OrderTable[2].ToString();
                    CustomerOrder.EmployeeCode = OrderTable[3].ToString();
                    CustomerOrder.ProductCode = OrderTable[4].ToString();
                    CustomerOrder.OrderDatestr = Convert.ToDateTime(OrderTable[5]);
                    CustomerOrder.NoFoIteams = Convert.ToInt32(OrderTable[6]);
                    CustomerOrder.Amount = Convert.ToDecimal(OrderTable[7]);


                    CustomerOrder.OrderBillings = new List<BillingModel>();

                    foreach (DataRow billingtable in dtBilling.Rows)
                    {
                        BillingModel OrderBills = new BillingModel();
                        OrderBills.BillingNumber = Convert.ToString(billingtable[0]);
                        OrderBills.ModeOfPayment = Convert.ToString(billingtable[1]);
                        OrderBills.Ordercode = Convert.ToString(billingtable[2]);
                        OrderBills.BillingDate = Convert.ToDateTime(billingtable[3]);
                        OrderBills.Amount = Convert.ToDecimal(billingtable[4]);
                        CustomerOrder.OrderBillings.Add(OrderBills);
                    }
                    Customermodel.custumerOrders.Add(CustomerOrder);
                }
            }
        }
    }
}
