using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_Models;
using RSC_Configurations;
using System.Data;
using RSC_DataAccess;
using RSC_IDAL;

namespace RSC_FileProcessor
{
    public class EmployeProcesser
    {
        private string EmployeFilePath { get; set; }
        private int Storeid { get; set; }
        private string[] FileContant { get; set; }
        private string FailReason { get; set; }
        private string Storecode { get; set; }
        private bool isValidFile { get; set; }
        private List<EmployeeModel> EmpData { get; set; }
        private IEmployeeDAL EmployeeObj { get; set; }

        public EmployeProcesser(string employeefilepath, int storeid, string storecode, IEmployeeDAL employeeDALObj)
        {
            EmployeeObj = employeeDALObj;
            EmployeFilePath = employeefilepath;
            Storeid = storeid;
            Storecode = storecode;

        }
        public void processor()
        {
            ReadEmployeeData();
            ValidateDate();
            PushEmployeeDataToDB();
        }

        private void ReadEmployeeData()
        {
            FileContant = File.ReadAllLines(EmployeFilePath);
        }

        private void ValidateDate()
        {
            if (FileContant.Length < 1)
            {
                FailReason = "there is no data in  EmployeeFile";
            }
            if (FileContant.Length == 1)
            {
                FailReason = "In the table only colomns ia avaliable but not data there";
            }
            else
            {
                for (int i = 1; i < FileContant.Length; i++)
                {
                    string[] Data = FileContant[i].Split('|');

                    if (Data.Length != 9)
                    {
                        FailReason = "Log the error: Invalid data; Not matching with noOffieds expected i.e., 6.";
                        break;
                    }

                    if (Data[1].ToLower() != Storecode.ToLower())
                    {
                        FailReason = "Log the error: Invalid data; store code not matching with current storecode.;";
                    }

                    if (!DateTime.TryParse(Data[4], out DateTime Joindt))
                    {
                        FailReason += $"Error: invalid date; value:{Data[4]};recordNumber:{i};";
                    }

                    if (DateTime.TryParse(Data[5], out DateTime Leavedt1))
                    {
                        FailReason += $"Error: invalid date; value:{Data[5]};recordNumber:{i};";
                    }
                }
            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                FileHelper.MoveFile(EmployeFilePath, FileStatus.Failure);
            }
            isValidFile = true;
        }
        private void PushEmployeeDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
            else
            {
                bool ISSuccess = EmployeeObj.EmployeeDBAcces(EmpData, Storeid);
                if (ISSuccess)
                {
                    FileHelper.MoveFile(EmployeFilePath, FileStatus.Success);
                }
                else
                {
                    FileHelper.MoveFile(EmployeFilePath, FileStatus.Failure);
                }
            }
        }
        private void PrepareEmployeeObjects()
        {
            EmpData = new List<EmployeeModel>();
            for (int i = 1; i < FileContant.Length; i++)
            {
                string Record = FileContant[i];
                string[] data = Record.Split('|');
                EmployeeModel model = new EmployeeModel();
                model.EmpCode = data[0];
                model.Storeidfk = this.Storeid;
                model.EmployeeName = data[2];
                model.Role = data[3];
                model.DateOfJoiningstr = data[4];
                model.DateOfLeavingstr = data[5];
                model.ContactNumber = data[6];
                model.Gender = data[7];
                if (decimal.TryParse(data[8], out decimal price))
                    model.Salary = price;
                EmpData.Add(model);
            }
        }
    }
}


