using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataAccess;
using RSC_IDAL;

namespace RSC_BusinessLayer
{
    public class StoreProcesser
    {
        private bool isValidFile { get; set; }
        private string FailReason { get; set; }
        private string storeFilePath { get; set; }
        private string DirName { get { return Path.GetFileName(Path.GetDirectoryName(storeFilePath)); } }
        private string[] FileContent { get; set; }
        private List<storemodel> store { get; set; }
        public IstoreDA StoreDALObj { get; set; }
        public static List<storemodel> stores
        {
            get
            {
                return storeprocessDA.GetAllStoresFromDB();
            }
        }
        public StoreProcesser(string filePath, IstoreDA storeObj)
        {
            storeFilePath = filePath;
            StoreDALObj = storeObj;
        }

        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();
        }



        private void ReadFileData()
        {
            FileContent = File.ReadAllLines(storeFilePath);
        }

        private void ValidateStoreData()
        {
            //validate if file has no content
            if (FileContent.Length < 1)
            {
                FailReason = "Log the error: Invalid file";
            }
            //validate if file has only header row
            else if (FileContent.Length == 1)
            {
                FailReason = "Log the warning: No data present in the file";
            }
            else if (FileContent.Length > 2)
            {
                FailReason = "Log the error: Invalid file; has multiple store records.";
            }
            else
            {

                string[] data = FileContent[1].Split(',');
                if (data.Length != 6)
                {
                    FailReason = "Log the error: Invalid data; Not matching with noOffieds expected i.e., 6.";
                }
                else if (data[1].ToLower() != DirName.ToLower())
                {
                    FailReason = "Log the error: Invalid data; store code not matching with current storecode.";
                }
            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                //log this error message into a file
                return;
            }
            isValidFile = true;
        }
        private void PushStoreDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
            PrepareStoreObject();
            StoreDALObj.syncstoreTabledata(store);
        }

        public void PrepareStoreObject()
        {
            store = new List<storemodel>();
            string[] data = FileContent[1].Split(',');
            storemodel modelObj = new storemodel();
            modelObj.StoreCode = data[1];
            modelObj.StoreName = data[2];
            modelObj.Location = data[3];
            modelObj.ManagerName = data[4];
            modelObj.ContactNumber = data[5];
            stores.Add(modelObj);
            //  storeprocessDA spda=new storeprocessDA();
            // spda.GetAllStoresFromDB();
        }
    }
}
