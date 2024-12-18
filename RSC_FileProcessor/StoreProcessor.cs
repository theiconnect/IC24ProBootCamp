using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using RSC_Configurations;
using RSC_Models;
using RSC_DataAccess;

namespace RSC_FileProcessor
{
    public class StoreProcessor 
    {
        private string StoreFilePath { get; set; }
        private string StoreCode { get; set; }
        private int StoreId { get; set; }
        private string[] filecontent { get; set; }
        private string FailReason { get; set; }
        private List<StoreModel> StoreData { get; set; }
        public static List<StoreModel> DBStorecodes 
        { 
            get 
            {
                return GetStoreDataFromDB.GetStorecodesFromDB();
            }  
        }
        public StoreProcessor(string Storefilepath, string storecode, int storeid)
        {
            StoreFilePath = Storefilepath;
            StoreCode = storecode;
            StoreId = storeid;
        }
        public void Processor()
        {
            ReadStoreData();
            ValidateStoreData();
            PrepareStoreObject();
        }
        private void ReadStoreData()
        {
            filecontent = File.ReadAllLines(StoreFilePath);
        }

        private void ValidateStoreData()
        {
            if (filecontent.Length < 1)
            {
                FailReason = "log the error:invalid file";

            }
            else if (filecontent.Length == 1)
            {
                FailReason = "log the warning:no data found";

            }
            else if (filecontent.Length > 2)
            {
                FailReason = "log the error: invalid file has multiple store records";

            }

            string[] data = filecontent[1].Split(',');

            if (data.Length != 6)
            {
                FailReason = "log the error: invalid data; not matching  with no of fields expected";

            }
                
            data = filecontent[1].Split(',');
            StoreModel model = new StoreModel();
            model.StoreCode = data[1];
            model.StoreName = data[2];
            model.Location = data[3];
            model.ManagerName = data[4];
            model.ContactNumber = data[5];
            StoreData.Add(model);
            if (model.StoreCode.ToLower() != StoreCode.ToLower())
            {
                Console.WriteLine("log the error: invalid data; store codenot matching with current strorecode");
            }
        }
        private void PrepareStoreObject()
        {
          string[]  data = filecontent[1].Split(',');
            StoreModel model = new StoreModel();
            model.StoreCode = data[1];
            model.StoreName = data[2];
            model.Location = data[3];
            model.ManagerName = data[4];
            model.ContactNumber = data[5];

            new StoreDBAccess(model);

        }
    }
}
