using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using DataAccess;
using IDataAccess;


namespace BusinessAccessLayer
{
    public class EmployeeProcessor
    {
        private string[] employeeFileContent { get; set; }
        private string EmployeeFilePath { get; set; }
        private string FailReason { get; set; }
        private bool isValidFile { get; set; }
        private string DirName { get { return Path.GetFileName(Path.GetDirectoryName(EmployeeFilePath)); } }
        private string[] employeeFileData { get; set; }
        private int StoreIdFk { get; set; }
        private List<EmployeeDTO> fileEmployeeDTOObject { get; set; }
        private List<EmployeeDTO> employeeData { get; set; }
        private IEmployeeDA objEmployeeDA {  get; set; }

        public EmployeeProcessor(string employeeFilePath, IEmployeeDA objIEmployeeDA)
        {
            EmployeeFilePath = employeeFilePath;
            objEmployeeDA = objIEmployeeDA;

        }
        public void Process()
        {
            ReadFileData();
            ValidateEmployeeData();
            PushEmployeeDataToDB();
            FileHelper.Move(EmployeeFilePath, FileStatus.Sucess);
        }

        private void ReadFileData()
        {
            employeeFileContent = File.ReadAllLines(EmployeeFilePath);
        }

        private void ValidateEmployeeData()
        {
            //validate if file has no content
            if (employeeFileContent.Length < 1)
            {
                isValidFile = false;
                FailReason = "Log the error: Invalid file";
            }
            //validate if file has only header row
            else if (employeeFileContent.Length == 1)
            {
                isValidFile = false;
                FailReason = "Log the warning: No data present in the file";
            }
            else
            {
                for (int i = 1; i < employeeFileContent.Length; i++)
                {
                    string[] employeeFileData = employeeFileContent[1].Split('|');
                    if (employeeFileData.Length != 9)
                    {
                        isValidFile = false;
                        FailReason = "Log the error: Invalid data; Not matching with noOffieds expected i.e., 9 fileds data.";

                    }
                    if (employeeFileData[1].ToLower() != DirName.ToLower())
                    {
                        isValidFile = false;
                        FailReason = "Log the error: Invalid data; store code not matching with current storecode.;";
                    }
                    if (!DateTime.TryParse(employeeFileData[4], out DateTime DateOfJoining))
                    {
                        FailReason += $"Error: invalid Date; value:{employeeFileData[4]};recordNumber:{i} ;";
                    }


                    if (!DateTime.TryParse(employeeFileData[5], out DateTime dateOfJoining))
                    {
                        FailReason += $"Error: invalid Date; value:{employeeFileData[5]};recordNumber:{i} ;";
                    }
                    if (!decimal.TryParse(employeeFileData[8], out decimal Salary))
                    {
                        FailReason += $"Error: invalid type; value:{employeeFileData[8]};recordNumber:{i} ;";
                    }


                }


            }
            if (!string.IsNullOrEmpty(FailReason))
            {
                isValidFile = false;
                //log this error message into a file
                return;
            }
            isValidFile = true;

        }

        private void PushEmployeeDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
            PrepareEmployeeModelObject();
            //EmployeeDA employeeObj = new EmployeeDA();
            
            objEmployeeDA.SyncEmployeeDataWithDB(fileEmployeeDTOObject, StoreIdFk);


        }
        private void PrepareEmployeeModelObject()
        {
            //check this code once
            //it giving wrong way output
            fileEmployeeDTOObject = new List<EmployeeDTO>();
            for (int i = 1; i < employeeFileContent.Length; i++)
            {
                employeeFileData = employeeFileContent[i].Split('|');
                EmployeeDTO model = new EmployeeDTO();
                model.EmployeeCode = employeeFileData[0];
                model.StoreCode = employeeFileData[1];
                model.EmployeeName = employeeFileData[2];
                model.Role = employeeFileData[3];
                if (DateTime.TryParse(employeeFileData[4], out DateTime dateOfJoining))
                    model.DateOfJoining = dateOfJoining;
                if (DateTime.TryParse(employeeFileData[5], out DateTime dateOfLeaving))
                    model.DateOfLeaving = dateOfLeaving;
                model.ContactNumber = employeeFileData[6];
                model.Gender = employeeFileData[7];
                if (decimal.TryParse(employeeFileData[8], out decimal salary))
                    model.Salary = salary;
                fileEmployeeDTOObject.Add(model);
            }

        }
    }
}
