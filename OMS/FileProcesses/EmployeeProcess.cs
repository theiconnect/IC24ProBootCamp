using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using FileModel;
using ProjectHelpers;
using Configuration;
using DBDataAcesses;
using Enum;
using OMS_IDAL;
using OMSEntityDAL;
namespace FileProcesses
{
    public class EmployeeProcess:BaseProcessor
    {
       
        private bool isValidFile { get; set; }

        private string EmployeeFilePath {  get; set; }
        private string FailedReason { get; set; }
        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(EmployeeFilePath)); }
        }

        private List<EmployeeModel > EmployeesList;


        public EmployeeProcess( string Employeefile) 
        
        {
        
            EmployeeFilePath = Employeefile;
        }

        public void Process()
        {

            ReadFileData();
            ValidateStoreData();
            //IEmployeeyDAL employeeyDAL = new EmployeeDAL();
            IEmployeeyDAL employeeyDAL=new EmployeeEntityDAL();
            employeeyDAL.PushEmployeeDataToDB(EmployeesList, EmployeeFilePath);


        }

        private void ReadFileData()
        {
            DataSet dsEmployees = FileHelper.GetXMLFileContent(EmployeeFilePath);
            PrepareEmployeeModel(dsEmployees);
            
        }

        private void PrepareEmployeeModel(DataSet dsEmployees)
        {
            EmployeesList = new List<EmployeeModel>();
            try
            {
                foreach (DataRow employeeRecord in dsEmployees.Tables[0].Rows)
                {
                    EmployeeModel employeeModel = new EmployeeModel();
                    employeeModel.EmpCode = Convert.ToString(employeeRecord["EmployeeCode"]);
                    employeeModel.EmpName = Convert.ToString(employeeRecord["EmployeeName"]);
                    employeeModel.EmpWareHouseCode = Convert.ToString(employeeRecord["WarehouseCode"]);
                    employeeModel.EmpContactNumber = Convert.ToString(employeeRecord["ContactNumber"]);
                    employeeModel.Gender = Convert.ToString(employeeRecord["Gender"]);
                    employeeModel.Salary = Convert.ToString(employeeRecord["Salary"]);
                    EmployeesList.Add(employeeModel);

                }
            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(EmployeeFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }

        }

        private void ValidateStoreData()
        {
            try
            {
                foreach (var employeeRecord in EmployeesList)
                {
                    if (string.IsNullOrEmpty(employeeRecord.EmpCode))
                    {
                        employeeRecord.IsValidEmpolyee = false;
                        continue;
                    }
                    if (string.IsNullOrEmpty(employeeRecord.EmpWareHouseCode))
                    {
                        employeeRecord.IsValidEmpolyee = false;
                        continue;
                    }

                    if (employeeRecord.EmpWareHouseCode != dirName)
                    {
                        employeeRecord.IsValidEmpolyee = false;
                        continue;
                    }

                }
            }
            catch (Exception ex)
            {
                FileHelper.MoiveFile(EmployeeFilePath, FileStatus.Failure);

                Console.WriteLine(ex.Message);
            }

        }

       

    }

}
