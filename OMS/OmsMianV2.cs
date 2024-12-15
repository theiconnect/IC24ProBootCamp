//using SampleApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FileModel;
using FileProcesses;
using Enum;


namespace OMS
{
    internal class OmsMianV2 : ConfigHelper
    {
        static void Main()
        {
            string[] wareHouseFolders = Directory.GetDirectories(RootFolderPath);
            List<WareHouseModel> wareHouses = WareHouseProcess.GetAllWareHouses();

            foreach (string folderPath in wareHouseFolders)
            {
                string wareHouseFolderName = FileHelper.GetDirectoryNameByDirectoryPath(folderPath);
                var warehouse = wareHouses.FirstOrDefault(x => x.WareHouseCode == wareHouseFolderName);

                if (warehouse == null)
                {
                    Console.WriteLine("Invalid Warehouse Code");
                    continue;
                }

                string WareHouseFile = GetFileNameByFileType(folderPath, FileTypes.wareHouse);


                if (!string.IsNullOrEmpty(WareHouseFile))
                {
                    new WareHouseProcess(WareHouseFile).process();
                }

                string EmployeeFile = GetFileNameByFileType(folderPath, FileTypes.employee);

                if (!string.IsNullOrEmpty(EmployeeFile))
                {
                    new EmployeeProcess(EmployeeFile).Process();
                }

                string InventoryFile = GetFileNameByFileType(folderPath, FileTypes.inventory);

                if (!string.IsNullOrEmpty(InventoryFile))
                {
                    new InventoryProcess(InventoryFile).Process();
                }

                string CustomersFile = GetFileNameByFileType(folderPath, FileTypes.customers);

                string OrdersFile = GetFileNameByFileType(folderPath, FileTypes.orders);

                string OrderItemsFile = GetFileNameByFileType(folderPath, FileTypes.orderitem);
                
                new CustomerProcess(CustomersFile, OrdersFile, OrderItemsFile, warehouse.WareHouseidpk).Process();

                string returnFilePath= GetFileNameByFileType(folderPath,FileTypes.returns);
                new ReturnProcess(returnFilePath, warehouse.WareHouseidpk).Process();

            }

            Console.ReadLine();

        }

        private static string GetFileNameByFileType(string folder, FileTypes filetype)
        {
            string[] wareHouseLevelFile = Directory.GetFiles(folder);
            string StartsWith = string.Empty;

            switch (filetype)
            {
                case FileTypes.wareHouse:
                    StartsWith = "warehouse"; break;
                case FileTypes.employee:
                    StartsWith = "employee"; break;
                case FileTypes.inventory:
                    StartsWith = "inventory"; break;
                case FileTypes.customers:
                    StartsWith = "customers";break;
                case FileTypes.orders:
                    StartsWith = "orders"; break;
                case FileTypes.orderitem:
                    StartsWith = "orderitem";break;
                case FileTypes.returns:
                    StartsWith = "returns";break;


            }

            foreach (string file in wareHouseLevelFile)
            {
                if (Path.GetFileNameWithoutExtension(file).ToLower().Trim().StartsWith(StartsWith))
                {
                    return file;
                }
            }
            return null;
        }

        
    }
}
