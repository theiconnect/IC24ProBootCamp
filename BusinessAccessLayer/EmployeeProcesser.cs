﻿using RSC.AppConnection_Kiran;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using RSC.FileModel_Kiran;
using RSC_Validations;
using DataBaseAccessLayer;
using RSC_IDAL;

namespace BusinessAccessLayer
{

    public class Employeeprocesser
    {
        public int StoreId { get; set; }
        private bool isValidFile { get; set; }
        private string FailReason { get; set; }
        private string FilePath { get; set; }
        private string DBEmployees { get; set; }
        private string DirName { get { return Path.GetDirectoryName(FilePath); } }
        private string[] FileContent { get; set; }
        private string StoreCode { get; set; }
        public IEmployeeDAL EmployeeObj { get; set; }
        public List<EmployeeModel> employees { get; set; }
        public Employeeprocesser(string filePath, int storeId, string Storecode ,IEmployeeDAL employeeDALObj)
        {
            EmployeeObj = employeeDALObj;
            FilePath = filePath;
            StoreId = storeId;
            StoreCode = Storecode;
        }
        public void processer()
        {
            ReadFileData();
            EmployeeValidations();
            PrepareEmployeeObject();

            EmployeeObj.PushStoreDataToDB(employees, StoreId);
        }

        public void EmployeeValidations()
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
            throw new NotImplementedException();
        }

        private void ReadFileData()
        {
            FileContent = File.ReadAllLines(FilePath);
            //EmployeeValidations obj = new EmployeeValidations();
            //obj.ValidateEmployeeData(FileContent, StoreId);
        }
        //public class EmployeeValidations()
        
            //private string[] FileContent { get; set; }
            //public int StoreId { get; set; }
            //public void ValidateEmployeeData(string[] fileContent, int storeId)
            //{
            //    this.FileContent = fileContent;
            //    this.StoreId = storeId;

              
               
            //}
            private void PrepareEmployeeObject()
            {
                for (int i = 1; i < FileContent.Length; i++)
                {
                    var Employees = new List<EmployeeModel>();
                    string[] data = FileContent[1].Split('|');
                    EmployeeModel Model = new EmployeeModel();
                    Model.employeeCode = data[1];
                    Model.employeeName = data[2];
                    Model.employeeRole = data[3];
                    Model.employeeDateOfJoining = Convert.ToDateTime(data[4]);
                    Model.employeeDateOfLeaving = Convert.ToDateTime(data[5]);
                    Model.employeeContactNumber = data[6];
                    Model.employeeGender = data[7];
                    Model.employeeSalary = Convert.ToDecimal(data[8]);

                    Employees.Add(Model);
                    SyncEmployeePushToDB obj = new SyncEmployeePushToDB();
                    obj.PushStoreDataToDB(employees, StoreId);

                }

            }
        

    }

}
