using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Configuration;
using FileModel;
using ProjectHelper;
using FilesEnum;
using FileProcessses;
using System.Configuration;
using OMS.IDataAccessLayer_Muni;
using OMS.DataAccessLayer;
using OMS.DataAccessLayer.Entity_Muni;
using OMS.DataAccessLayer_Muni;
namespace OMS
{
    internal class OmsMianV2 : ConfigHelper
    {
        private static IWareHouseDAL objWhDal { get; set; }
        private static IEmployeeDAL objEmpDal { get; set; }
        private static IInventoryDAL objInvDal { get; set; }
        private static WareHouseProcess objWhBAL { get; set; }
        private static EmployeeProcess objEmpBAL { get; set; }
        private static InventoryProcess objInvBAL { get; set; }

        static OmsMianV2()
        {
            if (UseEF)
            {
                objWhDal = new EntityWareHouseDAL();
                objEmpDal=new EntityEmployeeDAL();
                objInvDal=new EntityInventoryDAL();


            }
            else
            {
                objWhDal=new WareHouseDAL();
                objEmpDal=new EmployeeDAL();
                objInvDal=new InventoryDAL();

            }
        }
        static void Main()
        {
            string[] wareHouseFolders = Directory.GetDirectories(rootFolderPath);
           

            foreach (string folderPath in wareHouseFolders)
            {
                string WareHouseFile = GetFileNameByFileType(folderPath, FileTypes.wareHouse);
                

                objWhBAL=new WareHouseProcess(objWhDal, WareHouseFile);
                string wareHouseFolderName = FileHelper.GetDirectoryNameByDirectoryPath(folderPath);
                var warehouse = objWhBAL.wareHouses.FirstOrDefault(x => x.WareHouseCode == wareHouseFolderName);

                if (warehouse == null)
                {
                    Console.WriteLine("Invalid Warehouse Code");
                    continue;
                }

                if (!string.IsNullOrEmpty(WareHouseFile))
                {
                    objWhBAL.process();
                }

                string EmployeeFile = GetFileNameByFileType(folderPath, FileTypes.employee);
                objEmpBAL = new EmployeeProcess(objEmpDal, EmployeeFile);

                if (!string.IsNullOrEmpty(EmployeeFile))
                {
                    objEmpBAL.Process();
                }

                string InventoryFile = GetFileNameByFileType(folderPath, FileTypes.inventory);
                objInvBAL = new InventoryProcess(objInvDal, InventoryFile);

                if (!string.IsNullOrEmpty(InventoryFile))
                {
                    objInvBAL.Process();
                }

                string CustomersFile = GetFileNameByFileType(folderPath, FileTypes.customers);

                string OrdersFile = GetFileNameByFileType(folderPath, FileTypes.orders);

                string OrderItemsFile = GetFileNameByFileType(folderPath, FileTypes.orderitem);
                
                new CustomerProcess(CustomersFile, OrdersFile, OrderItemsFile, warehouse.WareHouseidpk).Process();

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
