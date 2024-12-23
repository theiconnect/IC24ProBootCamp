using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Configuration;
using FileModel;
using OMS.IDataAccessLayer_Muni;
using ProjectHelper;

namespace FileProcessses
{
    public class EmployeeProcess:DBHelper
    {
       
        private bool isValidFile { get; set; }=false;

        private string EmployeeFilePath {  get; set; }
        private string FailedReason { get; set; }
        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(EmployeeFilePath)); }
        }

        private List<EmployeeModel > employees;
        private IEmployeeDAL objEmpDAL { get; set; }
        public EmployeeProcess( IEmployeeDAL objEmpDal,string Employeefile) 
        
        {
        
            EmployeeFilePath = Employeefile;
            objEmpDAL = objEmpDal;
        }

        public void Process()
        {

            //READ
            //VALIDATE
            //PUSH INTO DB
            ReadFileData();
            ValidateEmployeeData();
            if (!isValidFile)
            {
                Console.WriteLine("Log the error:Employee  File is not valid");
                return;
            }
           
            objEmpDAL.PushEmployeeDataToDB(employees);


        }

        private void ReadFileData()
        {
            DataSet dsEmployees = FileHelper.GetXMLFileContent(EmployeeFilePath);
            PrepareEmployeeModel(dsEmployees);

        }

        private void PrepareEmployeeModel(DataSet dsEmployees)
        {
            employees = new List<EmployeeModel>();
            try
            {
                foreach (DataRow employeeRecord in dsEmployees.Tables[0].Rows)
                {
                    EmployeeModel employeeModel = new EmployeeModel();
                    employeeModel.EmpCode = Convert.ToString(employeeRecord["EmployeeCode"]);
                    employeeModel.EmpName = Convert.ToString(employeeRecord["EmployeeName"]);
                    employeeModel.EmpWareHouseCode = Convert.ToString(employeeRecord["WarehouseCode"]);
                    employeeModel.empContactNumber = Convert.ToString(employeeRecord["ContactNumber"]);
                    employeeModel.Gender = Convert.ToString(employeeRecord["Gender"]);
                    employeeModel.Salary = Convert.ToString(employeeRecord["Salary"]);
                    employees.Add(employeeModel);

                }
            }
            catch (Exception ex)
            {
                

                Console.WriteLine(ex.Message);
            }

        }

        private void ValidateEmployeeData()
        {

            foreach (var empdata in employees)
            {
                if (empdata.EmpWareHouseCode != dirName)
                {
                    FailedReason = "invalid warehouse code ";
                   // Console.WriteLine(FailedReason);
                    return;
                    
                }
            }
            isValidFile = true;

        }

        

       
    }
}
