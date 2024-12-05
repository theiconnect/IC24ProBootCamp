using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    internal class StoreProcessor : BaseProcessor
    {
        private bool isValidFile { get; set; }
        private string FailReason { get; set; }
        private string FilePath { get; set; }
        private string DirName { get { return Path.GetDirectoryName(FilePath); } }
        private string[] FileContent { get; set; }
        private StoreModel StoreModel { get; set; }
        public StoreProcessor(string filePath)
        {
            FilePath = filePath;
        }

        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();
        }

        private void ReadFileData()
        {
            FileContent = File.ReadAllLines(FilePath);
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
            if(!isValidFile)
            {
                return;
            }

            PrepareStoreObject();

            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                string query = $"Update Stores Set StoreName = @StoreName, Location = @Location, ManagerName= @Manager, ContactNumber = @ContactNumber where StoreCode = @StoreCode";
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@StoreName", DbType.String).Value = StoreModel.StoreName;
                    cmd.Parameters.Add("@StoreCode", DbType.String).Value = StoreModel.StoreCode;
                    cmd.Parameters.Add("@Location", DbType.String).Value = StoreModel.Location;
                    cmd.Parameters.Add("@Manager", DbType.String).Value = StoreModel.ManagerName;
                    cmd.Parameters.Add("@ContactNumber", DbType.String).Value = StoreModel.ContactNumber;
                    int rowsaffected = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private void PrepareStoreObject()
        {
            string[] data = FileContent[1].Split(',');
            StoreModel.StoreCode = data[1];
            StoreModel.StoreName = data[2];
            StoreModel.Location = data[3];
            StoreModel.ManagerName = data[4];
            StoreModel.ContactNumber = data[5];
        }
    }
}
