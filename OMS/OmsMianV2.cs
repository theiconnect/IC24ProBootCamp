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
        private static IWareHouseDAL objWhDal { get; set; }
        private static IEmployeeyDAL objEmpDal { get; set; }
        private static WareHouseProcess objBal;
        static OmsMianV2()
        {
            if (UseEf)
            {
                objWhDal = new WareHouseEntityDAL();
                objEmpDal = new EmployeeEntityDAL();
            }
            else
            {
                objWhDal = new WarehouseDAL();
                objEmpDal = new EmployeeDAL();
            }
        }
        static void Main()
        {
            string[] wareHouseFolders = Directory.GetDirectories(RootFolderPath);
            


            foreach (string folderPath in wareHouseFolders)
            {
                string WareHouseFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.wareHouse);
                
                objBal = new WareHouseProcess(WareHouseFile, objWhDal);
                List<WareHouseModel> wareHouses = objBal.GetAllWareHouses();

                string wareHouseFolderName = FileHelper.GetDirectoryNameByDirectoryPath(folderPath);
                var warehouse = wareHouses.FirstOrDefault(x => x.WareHouseCode == wareHouseFolderName);

                if (warehouse == null)
                {
                    Console.WriteLine("Invalid Warehouse Code");
                    continue;
                }



                if (!string.IsNullOrEmpty(WareHouseFile))
                {
                    
                    objBal.process();
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
