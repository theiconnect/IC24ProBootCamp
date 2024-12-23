using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using BusinessAccessLayer;
using ConnectionConfig;
using FileTypes;
using FileHelper;
using DataAccessLayer;
using EnityDataAccessLayer;
using OMS_IDataAccessLayer;
using BusinessAccessLayer1;
using EnityDataAccessLayer.EntityFramework;
using OMSEnityDataAccessLayer;
using System.Diagnostics;

namespace OMS_Arjun_V3
{
    internal class Program 
    {
        public static IWareHouseDAL objWhDal { get; set; }
        public static IEmployeeDAL objEmpDal { get; set; }
        public static Process objBal;
        public bool UseEntityFramework {  get; set; }
        public void Main(string[] args)
        {
            if (UseEntityFramework)
            {
                objWhDal = new EnityWareHouseDAL();
                objEmpDal = new EnityEmployeeDAL();
            }
            else
            {
                objWhDal = new WareHouseDAL();
                objEmpDal = new EmployeeDAL();
            }
            string[] wareHouseFolders = Directory.GetDirectories(ConnectionConfig.ConnectionConfig1.rootFolderPath);
            List<WareHouseModel> wareHouses = objWhDal.getAllWareHousesFromDB();

            foreach (string folderPath in wareHouseFolders)
            {
                string wareHouseFolderName = FileHelper.FileHelper.GetDirectoryNameByDirectoryPath(folderPath);
                var warehouse = wareHouses.FirstOrDefault(x => x.WareHouseCode == wareHouseFolderName);

                if (warehouse == null)
                {
                    Console.WriteLine("Invalid Warehouse Code");
                    continue;
                }

                string WareHouseFile = GetFileNameByFileType(folderPath, FileTypes.FileTypes1.wareHouse);
                

                if (!string.IsNullOrEmpty(WareHouseFile))
                {
                    new WareHouseBAL(WareHouseFile, objWhDal).process();
                }

            }
        }
        private static string GetFileNameByFileType(string folder, FileTypes.FileTypes1 filetype)
        {
            string[] wareHouseLevelFile = Directory.GetFiles(folder);
            string StartsWith = string.Empty;

            switch (filetype)
            {
                case FileTypes.FileTypes1.wareHouse:
                    StartsWith = "warehouse"; break;
                case FileTypes.FileTypes1.employee:
                    StartsWith = "employee"; break;
                case FileTypes.FileTypes1.inventory:
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
