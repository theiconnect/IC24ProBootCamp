using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileModel;
using FileProcesses;
using Enum;
using Configuration;
using ProjectHelpers;
using DBDataAcesses;
using OMSDAL;
using OMS_IDAL;
using OMSEntityDAL;

namespace OMS
{
    internal class OmsMianV2 : ConfigHelper
    {
        static void Main()
        {
            string[] wareHouseFolders = Directory.GetDirectories(RootFolderPath);
            IGetAllWareHousesDataDAL getAllWareHousesDataDAL = new GetAllWareHousesDataEntityDAL();

           // IGetAllWareHousesDataDAL getAllWareHousesDataDAL = new GetAllWareHousesData();


            List<WareHouseModel> wareHouses = getAllWareHousesDataDAL.GetAllWareHouses();

            foreach (string folderPath in wareHouseFolders)
            {
                string wareHouseFolderName = FileHelper.GetDirectoryNameByDirectoryPath(folderPath);
                var warehouse = wareHouses.FirstOrDefault(x => x.WareHouseCode == wareHouseFolderName);

                if (warehouse == null)
                {
                    Console.WriteLine("Invalid Warehouse Code");
                    continue;
                }

                string WareHouseFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.wareHouse);


                if (!string.IsNullOrEmpty(WareHouseFile))
                {
                    new WareHouseProcess(WareHouseFile).process();
                }

                string EmployeeFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.employee);

                if (!string.IsNullOrEmpty(EmployeeFile))
                {
                    new EmployeeProcess(EmployeeFile).Process();
                }

                string InventoryFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.inventory);

                if (!string.IsNullOrEmpty(InventoryFile))
                {
                    new InventoryProcess(InventoryFile).Process();
                }

                string CustomersFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.customers);

                string OrdersFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.orders);

                string OrderItemsFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.orderitem);
                
                new CustomerProcess(CustomersFile, OrdersFile, OrderItemsFile, warehouse.WareHouseidpk).Process();

                string returnFilePath= FileHelper. GetFileNameByFileType(folderPath,FileTypes.returns);
                new ReturnProcess(returnFilePath, warehouse.WareHouseidpk).Process();

            }

            Console.ReadLine();

        }

       

        
    }
}
