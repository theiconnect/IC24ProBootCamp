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

namespace FileProcessses
{
    public class EmployeeProcess:DBHelper
    {
       
        private bool isValidFile { get; set; }

        private string EmployeeFilePath {  get; set; }
        private string FailedReason { get; set; }
        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(EmployeeFilePath)); }
        }

        private List<EmployeeModel > employees;
        public EmployeeProcess( string Employeefile) 
        
        {
        
            EmployeeFilePath = Employeefile;
        }

        public void Process()
        {

            //READ
            //VALIDATE
            //PUSH INTO DB
            ReadFileData();
            ValidateStoreData();
            PushEmployeeDataToDB();


        }

        private void ReadFileData()
        {
            employees=new List<EmployeeModel>();
            using (XmlReader  reader = XmlReader.Create(EmployeeFilePath))
            {
                EmployeeModel empmodel = new EmployeeModel();

                int count = 0;
                while (reader.Read())
                {


                    if (reader.IsStartElement()) // Check if it is an element node
                    {
                        if (reader.Name.ToLower() == "employeecode")
                        {
                            // Read element value
                            empmodel.EmpCode = reader.ReadElementContentAsString();
                            count++;
                            if (empmodel.EmpCode == "")
                            {
                                Console.WriteLine("Invalid employee record: EmployeeCode cannot be empty");
                                break;
                            }

                        }
                        else if (reader.Name.ToLower() == "employeename")
                        {
                            empmodel.EmpName = reader.ReadElementContentAsString();
                            count++;

                        }
                        else if (reader.Name.ToLower() == "warehousecode")
                        {
                            empmodel.EmpWareHouseCode = reader.ReadElementContentAsString();
                            count++;

                            if (empmodel.EmpWareHouseCode == "")
                            {
                                break;
                            }
                        }
                        else if (reader.Name.ToLower() == "contactnumber")

                        {
                            empmodel.empContactNumber = reader.ReadElementContentAsString();
                            count++;

                        }
                        else if ((reader.Name.ToLower() == "gender"))
                        {
                            empmodel.Gender = reader.ReadElementContentAsString();
                            count++;

                        }
                        else if (reader.Name.ToLower() == "salary")
                        {
                            empmodel.Salary = reader.ReadElementContentAsString();
                            count++;


                        }


                    }
                    if (count > 5&& empmodel.EmpCode!="")
                    {
                        

                        employees.Add(empmodel);
                        count = 0;
                        empmodel = new EmployeeModel();

                    }


                }
             
            }
            
        }

        private void ValidateStoreData()
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

        private void PushEmployeeDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
            using (SqlConnection conn = new SqlConnection(oMSConnectionString))
            {
                conn.Open();
                foreach(var data in employees)
                {
                    using (SqlCommand cmd = new SqlCommand("InsertOrUpdateEmpData", conn))
                    {
                        cmd.CommandType=CommandType.StoredProcedure;
                        cmd.Parameters.Add("@EmpCode",DbType.String).Value=data.EmpCode;
                        cmd.Parameters.Add("@EmpName",DbType.String).Value=data.EmpName;
                        cmd.Parameters.Add("@EmpWareHouseCode", DbType.String).Value = data.EmpWareHouseCode;
                        cmd.Parameters.Add("@EmpContactNumber", DbType.String).Value = data.empContactNumber;
                        cmd.Parameters.Add("@Gender", DbType.String).Value = data.Gender;
                        cmd.Parameters.Add("@Salary",DbType.Decimal).Value = data.Salary;

                        cmd.ExecuteNonQuery();

                    }
                }
               
            }
        }

       
    }
}
