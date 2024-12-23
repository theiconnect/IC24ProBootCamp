using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathAndDataBaseConfig;
using DataAccess;
using BusinessAccessLayer;
using IDataAccess;
using EntityDataAccess;

namespace RSC
{
    public class ProgramV2:BaseProcessor
    {
        static int storeIdPk { get; set; }
        private static IStoreDA objIStoreDA { get; set; }
        private static IEmployeeDA objIEmployeeDA { get; set; }
        private static IStockDA objIStockDA { get; set; }
        private static StoreProcessor objStoreBAL {  get; set; }
        public static EmployeeProcessor objEmployeeBAL { get; private set; }
        public static StockProcess objStockBAL { get; set; }

        static ProgramV2()
        {
            if (useEf)
            {
                objIStoreDA = new StoreEntityDA();
                objIEmployeeDA = new EmployeeEntityDA();
                objIStockDA = new StockEntityDA();

            }
            else
            {
                objIStoreDA = new StoreDA();
                objIEmployeeDA = new EmployeeDA();
                objIStockDA = new StockDA();


            }
        }
        static void Main(string[] args)
        {
            //Get all the store folders from root directory
            string[] directories = Directory.GetDirectories(BaseProcessor.mainFolderPath);

            //Get All Stores information From DB
            //StoreDA storeDAObject = new StoreDA();

            List<StoreModel> storesData = objIStoreDA.GetAllStoresDataFromDB();

            foreach (string storeDirectoryPath in directories)
            {
                //int Storeid = default;
                //Get Store Directory Path
                string storeDirName = Path.GetFileName(storeDirectoryPath);

                var store = storesData.Exists(x => x.StoreCode == storeDirName);
                //bool isvalid = false;
                //foreach(var stores in storesData)
                //{
                //    stores.StoreCode = storeDirName;
                //    stores.StoreIdPk = Storeid;
                //    isvalid = true;
                //    break;
                   

                //}
                if (!store)
                {
                    //there is no store found with the storecode matching with foldername leave that and go and check next storecode folder
                    continue;
                }
                //Get the storefile path from the directory
                string storeFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Stores); 
                // ////Initiate store file processing by using store processor

                objStoreBAL = new StoreProcessor(storeFilePath,objIStoreDA);
                objStoreBAL.Process();

                 string StockFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Stock);
                objStockBAL = new StockProcess(StockFilePath, storeIdPk,objIStockDA);
                 objStockBAL.Process();


                string EmployeeFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Employee);
                objEmployeeBAL = new EmployeeProcessor(EmployeeFilePath, objIEmployeeDA, storeIdPk);
                objEmployeeBAL.Process();

                //string CustomerFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Customer);
                //var customerProcessor = new CustomerProcess(CustomerFilePath, storeIdPk);
                //customerProcessor.Process();


            }
            Console.Read();
        }
    }
}
