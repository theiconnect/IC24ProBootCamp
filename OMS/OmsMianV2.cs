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
        private static IInventoryDAL objInventoryDal {  get; set; }
        private static ICustomerDAL objCustomerDal { get; set; }
        private static IReturnsDAL objReturnsDal { get; set; }

        private static WareHouseProcess objWhBal;
        private static EmployeeProcess objEmpBal;
        private static InventoryProcess objInvetoryBal;
        private static CustomerProcess objCustomerpBal;
        private static ReturnProcess objReturnsBal;
        static OmsMianV2()
        {
            if (UseEf)
            {
                objWhDal = new WareHouseEntityDAL();
                objEmpDal = new EmployeeEntityDAL();
                objInventoryDal= new InventoryEntityDAL();
                objCustomerDal= new CustomerEntityDAL();
                objReturnsDal=new ReturnsEntityDAL();

            }
            else
            {
                objWhDal = new WarehouseDAL();
                objEmpDal = new EmployeeDAL();
                //objInventoryDal = new InventoryDAL();
                objCustomerDal=new CustomersDAL();
                objReturnsDal= new ReturnsDAL();
                
            }
        }
        static void Main()
        {
            string[] wareHouseFolders = Directory.GetDirectories(RootFolderPath);
            


            foreach (string folderPath in wareHouseFolders)
            {
                
                string WareHouseFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.wareHouse);
                objWhBal = new WareHouseProcess(WareHouseFile, objWhDal);
                List<WareHouseModel> wareHouses = objWhBal.GetAllWareHouses();

                string wareHouseFolderName = FileHelper.GetDirectoryNameByDirectoryPath(folderPath);
                var warehouse = wareHouses.FirstOrDefault(x => x.WareHouseCode == wareHouseFolderName);

                if (warehouse == null)
                {
                    Console.WriteLine("Invalid Warehouse Code");
                    continue;
                }



                if (!string.IsNullOrEmpty(WareHouseFile))
                {
                    
                    objWhBal.process();
                }

                string EmployeeFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.employee);

                objEmpBal = new EmployeeProcess(EmployeeFile,objEmpDal);

                if (!string.IsNullOrEmpty(EmployeeFile))
                {
                    objEmpBal.Process();
                }


                string InventoryFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.inventory);

                objInvetoryBal = new InventoryProcess(InventoryFile,objInventoryDal);

                if (!string.IsNullOrEmpty(InventoryFile))
                {
                    objInvetoryBal.Process();
                }

                string CustomersFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.customers);

                string OrdersFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.orders);

                string OrderItemsFile = FileHelper.GetFileNameByFileType(folderPath, FileTypes.orderitem);

                objCustomerpBal = new CustomerProcess(CustomersFile, OrdersFile, OrderItemsFile, objCustomerDal, warehouse.WareHouseidpk);
                objCustomerpBal.Process();

                string returnFilePath= FileHelper. GetFileNameByFileType(folderPath,FileTypes.returns);

                objReturnsBal = new ReturnProcess(returnFilePath, warehouse.WareHouseidpk, objReturnsDal);
                objReturnsBal.Process();

            }

            Console.ReadLine();

        }

       

        
    }
}
