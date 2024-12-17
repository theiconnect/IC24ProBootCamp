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
using DataAcess;

namespace RSC
{
    public class ProgramV2
    {
        static int storeIdPk { get; set; }
        static void Main(string[] args)
        {
            //Get all the store folders from root directory
            string[] directories = Directory.GetDirectories(BaseProcessor.mainFolderPath);

            //Get All Stores information From DB
            List<StoreModel> storesData = StoreDA.GetAllStoresDataFromDB();

            foreach (string storeDirectoryPath in directories)
            {
                //Get Store Directory Path
                string storeDirName = Path.GetFileName(storeDirectoryPath);

                var store = storesData.Exists(x => x.StoreCode == storeDirName);

                if (!store)
                {
                    //there is no store found with the storecode matching with foldername leave that and go and check next storecode folder
                    continue;
                }
                //Get the storefile path from the directory
                string storeFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Stores); 
                //Initiate store file processing by using store processor
                var storeProcessor = new StoreProcessor(storeFilePath);
                storeProcessor.Process();

                string StockFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Stock);
                var stockProcessor = new StockProcess(StockFilePath, storeIdPk);
                stockProcessor.Process();


                string EmployeeFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Employee);
                var employeeProcessor = new EmployeeProcessor(EmployeeFilePath);
                employeeProcessor.Process();

                string CustomerFilePath = FileHelper.GetFileNameByFileType(storeDirectoryPath, FileTypes.Customer);
                var customerProcessor = new CustomerProcess(CustomerFilePath, storeIdPk);
                customerProcessor.Process();


            }
            Console.Read();
        }
    }
}
