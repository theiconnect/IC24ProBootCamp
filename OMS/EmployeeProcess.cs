using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;

namespace OMS
{
    internal class EmployeeProcess:BaseProcessor
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

        internal void Process()
        {

            //READ
            //VALIDATE
            //PUSH INTO DB
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();


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

                    if (employeeRecord.EmpCode != dirName)
                    {
                        employeeRecord.IsValidEmpolyee = false;
                        continue;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void PushStoreDataToDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(oMSConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertOrUpdateEmpData", conn))
                    {
                        foreach (var employeeData in EmployeesList)
                        {
                            if (!employeeData.IsValidEmpolyee) continue;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@EmpCode", employeeData.EmpCode);
                            cmd.Parameters.AddWithValue("@EmpName", employeeData.EmpName);
                            cmd.Parameters.AddWithValue("@EmpWareHouseCode", employeeData.EmpWareHouseCode);
                            cmd.Parameters.AddWithValue("@EmpContactNumber", employeeData.EmpContactNumber);
                            cmd.Parameters.AddWithValue("@Gender", employeeData.Gender);
                            cmd.Parameters.AddWithValue("@Salary", employeeData.Salary);
                            cmd.ExecuteNonQuery();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }

}
