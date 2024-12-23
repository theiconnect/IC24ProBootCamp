using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RSC.Configuration_Venky;
using RSC.FileModels_Venky;

namespace RSC.BusinessAccessLayer_Venky
{
    public  class StoreProcesser 

    {
        public string StoreFilePath { get; set; }
        public string[] FileContent { get; set; }
        public List<StoreModel> stores { get; set; }

        public  StoreProcesser(string storefilepath)
        {
            StoreFilePath = storefilepath;
        }
        public void Processer()
        {
            ReadFileData();
            ValidateData();
            PrepareStoreObject();
        }
        private void ReadFileData()
        {
            FileContent = File.ReadAllLines(StoreFilePath);
        }

        private void ValidateData()
        {
            if (FileContent.Length < 1)
            {
                Console.WriteLine("Log the error: Invalid file");

            }
            //validate if file has only header row
            else if (FileContent.Length == 1)
            {
                Console.WriteLine("Log the warning: No data present in the file");

            }
            else if (FileContent.Length > 2)
            {
                Console.WriteLine("Log the error: Invalid file; has multiple store records.");

            }

            string[] data = FileContent[1].Split(',');


            if (data.Length != 6)
            {
                Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 6.");

            }
                
            
        }
        private void PrepareStoreObject()
        {
            var stores = new List<StoreModel>();
            string[] data = FileContent[1].Split(',');
            StoreModel Model = new StoreModel();
            Model.storeCode = data[1];
            Model.storeName = data[2];
            Model.location = data[3];
            Model.managerName = data[4];
            Model.contactNumber = data[5];
            stores.Add(Model);

            //PushStoreDataToDB(Model);
        }

       
    }
}
    
