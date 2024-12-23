using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSCModels;
using RSCDataAccess;


namespace RSC
{
    internal class StoreProcessor
    {
        private object storeData {  get; set; }

        public StoreProcessor(string storeFilePath)
        {
            StoreFilePath = storeFilePath;
        }

        private string[] FileContentLines { get; set; }
        private string StoreFilePath { get; set; }
        private string StoreDirName { get { return Path.GetFileName(Path.GetDirectoryName(StoreFilePath)); } }
        private string FailReason { get; set; }


        private bool isValidFile { get; set; }
        StoreModel StoreData { get; set; }
        private string StoreModelObj { get; set; }

        


        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            //PushStoreDataToDB();
            PushStoreDataToDB();
        }



        private void ReadFileData()
        {
            FileContentLines = File.ReadAllLines(StoreFilePath);


        }
        private void ValidateStoreData()
        {
            if (FileContentLines.Length < 1)
            {
                Console.WriteLine("Log The Error:Invalid File");
            }
            else if (FileContentLines.Length == 1)
            {
                Console.WriteLine("Log The Error:The File Has Only Header Row");
            }
            else if (FileContentLines.Length > 2)
            {
                Console.WriteLine("Log The Error:The File Has Multiple Store Records ");
            }
            else
            {
                string[] data = FileContentLines[1].Split(',');
                if (data.Length != 6)
                {
                    FailReason = "Log The Error:Invalid data;Not Matching With noOfFields expected i.e,6.";
                }
                else if (data[1].ToLower() != StoreDirName.ToLower())
                {
                    FailReason = "Log The Error:Invalid data;StoreCode Not Matching With Current StoreCode.";
                }


            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                return;
            }
            isValidFile = true;
        }

        private void PushStoreDataToDB()
        {
            if (!isValidFile)

            { return; }
            PrepareStoreObject();
            StoreDataAcess StoreObject = new StoreDataAcess();
            StoreObject.PushStoreData(StoreData);

        }



        public void PrepareStoreObject()
        {
            string[] data = FileContentLines[1].Split(',');
            StoreData = new StoreModel();
            StoreData.StoreId = Convert.ToInt32(data[0]);
            StoreData.StoreCode = data[1];
            StoreData.StoreName = data[2];
            StoreData.Location = data[3];
            StoreData.ManagerName = data[4];
            StoreData.ContactNumber = data[5];



            +
        }
    }
}
