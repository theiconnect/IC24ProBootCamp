using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Remoting.Messaging;
using RSCModels;
using System.Data.SqlClient;
using System.Management.Instrumentation;
using System.Data;
using RSCDataAccess;

namespace RSC
{
    internal class EmployeeProcessor
    {
        public string employeeFilePath {  get; set; }

        private String[] FileContentLines { get; set; }

        private string FailReason {  get; set; }

        private string StoreDirName {  get; set; }

        private EmployeeModel employeeData { get; set; }

        private bool IsValidFile { get; set; }

        public EmployeeProcessor(string employeeFilePath)
        {
            this.employeeFilePath = employeeFilePath;
        }

        public void Process()
        {
            ReadFileData();
            ValidateStoreData();
            PushEmployeeData();



        }
        public void PushEmployeeData()
        {
            if(!IsValidFile)
            {
                return;
            }
            PrepareEmployeeObject();
            EmployeeDataAcess employee = new EmployeeDataAcess();
            employee.PushEmployeeDataToDB(employeeData);

        }
        private void ReadFileData()
        {

            FileContentLines = File.ReadAllLines(employeeFilePath);

        }
        private void ValidateStoreData()
        {
            if(FileContentLines.Length<1)
            {
                Console.WriteLine(" Log The Error:Invalid File");
            }
            else if(FileContentLines.Length == 1)
            {
                Console.WriteLine("Log The Error:The File Has Only Header Row ");
            }
            else
            {
                for(int i = 0; i < FileContentLines.Length; i++)
                {
                    string[] data = FileContentLines[1].Split('|');
                    if (data.Length != 9)
                    {
                        FailReason = "Log The Error:Invalid data;Not Matching With noOfFields expected i.e,6.";
                    }
                    else if (data[1].ToLower() != StoreDirName.ToLower())
                    {
                        FailReason = "Log The Error:Invalid data;StoreCode Not Matching With Current StoreCode.";
                    }
                }
               


            }
            if(!string.IsNullOrEmpty(FailReason))
            {
                return;
            }

            IsValidFile = true;

        }
        

        private void PrepareEmployeeObject()
        {
           string[] data = FileContentLines[1].Split('|');
            employeeData = new EmployeeModel();
            employeeData.EmployeeCode = data[0];
            employeeData.StoreCode = data[1];
            employeeData.EmployeeName = data[2];
            employeeData.Role = data[3];
            employeeData.DateOfJoining = Convert.ToDateTime(data[4]);
            employeeData.DateOfLeaving = Convert.ToDateTime(data[5]);
            employeeData.ContactNumber = data[6];
            employeeData.Gender = data[7];
            employeeData.Salary = Convert.ToDecimal(data[8]);

        }



    }
}
