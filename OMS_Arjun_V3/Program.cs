using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_arjun;
using OMS_Arjun_V3.BusinessAccessLayer;

namespace OMS_Arjun_V3
{
    internal class Program : ConnectionConfig
    {

        static void Main(string[] args)
        {
            string[] wareHouseFolders = Directory.GetDirectories(rootFolderPath);
            List<WareHouseModel> wareHouses = WareHouseBAL.getAllWareHousesFromDB();

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
                    new WareHouseBAL(WareHouseFile).process();
                }

            }
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
