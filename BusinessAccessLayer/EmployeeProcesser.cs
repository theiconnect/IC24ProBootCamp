using RSC.AppConnection_Kiran;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using RSC.FileModel_Kiran;

namespace BusinessAccessLayer
{

    internal class Employeeprocesser : AppConnection
    {
        public int StoreId { get; set; }
        private bool isValidFile { get; set; }
        private string FailReason { get; set; }
        private string FilePath { get; set; }
        private string DBEmployees { get; set; }
        private string DirName { get { return Path.GetDirectoryName(FilePath); } }
        private string[] FileContent { get; set; }
        private string StoreCode { get; set; }
        public Employeeprocesser(string filePath, int storeId, string Storecode)
        {
            FilePath = filePath;
            StoreId = storeId;
            StoreCode = Storecode;

        }
        public void processer()
        {
            ReadFileData();
            ValidateStoreData();
            //PushStoreDataToDB();
        }
        private void ReadFileData()
        {
            FileContent = File.ReadAllLines(FilePath);
        }
        private void ValidateStoreData()
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

            if (data.Length != 9)
            {
                Console.WriteLine("Log the error: Invalid data; Not matching with noOffieds expected i.e., 9.");

            }
        }
        private void PrepareStoreObject()
        {

            var Employees = new List<EmployeeModel>();
            string[] data = FileContent[1].Split(',');
            EmployeeModel Model = new EmployeeModel();
            Model.employeeCode = data[1];
            Model.employeeName = data[2];
            Model.employeeRole = data[3];
            Model.employeeDateOfJoining = data[4];
            Model.employeeDateOfLeaving = data[5];
            Model.employeeContactNumber = data[6];
            Model.employeeGender = data[7];
            Model.employeeSalary = data[8];

            Employees.Add(Model);
            //PushStoreDataToDB(Model);
        }

       












    }

}
