using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Models;
using System.Data.SqlClient;
using System.Data;
using DataAccess;


namespace BusinessAccessLayer
{
    public class StoreProcessor
    {
        //variable names CamelCase and methods PascalCase
        private string[] fileContentLines { get; set; }
        private string storeFilePath { get; set; }
        private bool isValidFile { get; set; }
        private string failReason { get; set; }
        private string[] fileData { get; set; }
        private string storeDirName { get { return Path.GetFileName(Path.GetDirectoryName(storeFilePath)); } }
        private StoreModel storeModelObject { get; set; }
        public StoreProcessor(string FilePath)
        {
            storeFilePath = FilePath;
        }
        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();
            FileHelper.Move(storeFilePath,FileStatus.Sucess);
        }
        private void ReadFileData()
        {
            //read store file data
            fileContentLines = File.ReadAllLines(storeFilePath);

        }
        private void ValidateStoreData()
        {
            //validate file data Content
            //validate if file has no content
            if (fileContentLines.Length < 1)
            {

                isValidFile = false;//this line no need to write because bool default value is false
                failReason = "Log the error: Invalid file";
            }

            //validate if file has only header row
            else if (fileContentLines.Length == 1)
            {
                isValidFile = false;//this line no need to write because bool default value is false
                failReason = "Log the warning: No data present in the file.";
            }
            //validate if the store file having one store data only
            else if (fileContentLines.Length > 2)
            {
                isValidFile = false;//this line no need to write because bool default value is false
                failReason = "Log the error: Invalid file; has multiple store records.";
            }
            else
            {
                //In fileContyentLines Array---fileContyentLines[0] is headings line and data is 2 nd line onwards 
                fileData = fileContentLines[1].Split(',');
                if (fileData.Length != 6)
                {
                    isValidFile = false;
                    //after split the data is coming in array .in that all 6 elemnts are tbeir because in my store table 6 columns.6 elements not coming means error.
                    failReason = "Log the error: Invalid data; Not matching with noOffieds expected i.e., 6 fileds data.";
                }
                else if (fileData[1].ToLower() != storeDirName.ToLower())
                {
                    failReason = "Log the error: Invalid data; store code not matching with current storecode.";
                }


            }
            if (!string.IsNullOrEmpty(failReason))
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
            StoreDA.SyncStoreDataToDB(storeModelObject);
        }


        private void PrepareStoreObject()
        {
            fileData = fileContentLines[1].Split(',');
            storeModelObject = new StoreModel();

            //storeId is not taking because it is auto incremented file
            storeModelObject.StoreCode = fileData[1];
            storeModelObject.StoreName = fileData[2];
            storeModelObject.Location = fileData[3];
            storeModelObject.ManagerName = fileData[4];
            storeModelObject.ContactNumber = fileData[5];

        }
    }

}
