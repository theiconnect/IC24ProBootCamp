using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

using System.Threading;
using System.Data.OleDb;
using DataBaseConfig;
using DataAccess;
using Models;


namespace RSC_saikumar
{
    public class BatchJob 
    {
        static void Main(string[] args)
        {

            string[] pathfolderStoreCodes = Directory.GetDirectories(ConfigHelper.rootFolderPath);


            List<storemodel> stores = storeprocessDA.GetAllStoresFromDB();

            foreach (string folderstorecodes in pathfolderStoreCodes)
            {
                int storeId = default;

                string storeDirName = Path.GetFileName(folderstorecodes);

                bool isvalid = false;
                foreach (var x in stores)
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
                ////////////////////
                ///employee processer
                ///////////////
                ///
                new StoreProcessor(storeFilePath).Process();

                new EmployeProcesser(employeeFilePath, storeId, storeDirName).procesor();



                ///////////////////////
                ///store file processing
                //////////////////////
               
                

            }
        }





    }   
}

    

