using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_arjun;

namespace OMS_Arjun_v2
{
    internal class Program:ConConfigHelper
    {
        static void Main(string[] args)
        {
            string[] wareHouseFolders = Directory.GetDirectories(rootFolderPath);
            List<WareHouseModel> wareHouses = WareHouseProcessor.getAllWareHouses();

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
                    new WareHouseProcessor(WareHouseFile).process();
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

                new CustomerProcess(CustomersFile, OrdersFile, OrderItemsFile, warehouse.WareHouseIdpk).Process();

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
