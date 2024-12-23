using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSCConfigiration;
using RSCDataAccess;
using RSCModels;



namespace RSC
{
    internal class Program
    {       
        
        public static string Folder { get; private set; }
        static void Main(string[] args)
        {

            string[] StoreFolders = Directory.GetDirectories(AppConnection.rootFolder);
            foreach (string Folder in StoreFolders)
            {
                string StoreDirName = Path.GetFileName(Folder);


                List<StoreModel> models = StoreDataAcess.GetAllStoresFromDB();
                var store = models.Exists(x => x.StoreCode == StoreDirName);
                if (store == false)
                {
                    continue;
                }
                //string StoreFilePath = GetFileNameByFileType(Folder, FileTypes.Stores);
                //new StoreProcessor(StoreFilePath).Process();

                string EmployeeFilePath = GetFileNameByFileType(Folder, FileTypes.Employee);
                new EmployeeProcessor(EmployeeFilePath).Process();
            }

        }

        //private static string GetFileNameByFileType(string folder, FileTypes stores)
        //{

        //}

        private static string GetFileNameByFileType(string Folder, FileTypes filetype)
        {

            string[] StoreFiles = Directory.GetFiles(Folder);
            string StartsWithValue = string.Empty;
            switch (filetype)
            {
                case FileTypes.Stores:
                    StartsWithValue = "stores_"; break;

                case FileTypes.Stock:
                    StartsWithValue = "stock_"; break;

                case FileTypes.Employee:
                    StartsWithValue = "employee_"; break;

                case FileTypes.Customer:
                    StartsWithValue = "customer_"; break;
            }

            foreach (string file in StoreFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith(StartsWithValue))
                {
                    return file;
                }

            }
            return null;
        }
        
    
    }
}
