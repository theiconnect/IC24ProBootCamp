﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Configuration;
using System.Reflection;
using System.Collections;
using RSC_Models;
using RSC_Configurations;
using RSC_FileProcessor;


namespace RSC
{
    public class JobCreation 
    {
        static void Main(string[] args)
        {

            string[] pathfolderStoreCodes = Directory.GetDirectories(AppConfiguration.mainFolderPath);//get all paths on my local folders

            List<StoreModel> DBStorecodes = StoreProcessor.DBStorecodes;///get all storecodes by using DB

            foreach (string folderstorecodes in pathfolderStoreCodes)
            {
                int storeId = default;

                string storeDirName = Path.GetFileName(folderstorecodes);

                bool isvalid = false;
                foreach (var x in DBStorecodes)
                {
                    if (x.StoreCode == storeDirName)
                    {
                        storeId = x.StoreId;
                        isvalid = true;
                        break;
                    }
                }
                if (!isvalid)
                {
                    continue;
                }
                string[] storefolderfiles = Directory.GetFiles(folderstorecodes.ToString());
                string storeFilePath = string.Empty;
                string stockFilePath = string.Empty;
                string employeeFilePath = string.Empty;
                string customerFilePath = string.Empty;
                foreach (string file in storefolderfiles)
                {
                    if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("stores_"))
                    {
                        storeFilePath = file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("stock_"))
                    {
                        stockFilePath = file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("employee_"))
                    {
                        employeeFilePath = file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith("customer"))
                    {
                        customerFilePath = file;
                    }
                }

                new StockProcessor(storeFilePath, storeId, storeDirName).Processor();


                new StockProcessor(storeFilePath, storeId, storeDirName).Processor();


                new EmployeProcesser(employeeFilePath, storeId, storeDirName).processor();

                new CustumerProcessor(customerFilePath, storeId).processor();
            }
        }
    }
}
