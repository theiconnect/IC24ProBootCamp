using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_arjun;
using System.Xml;

namespace OMS_Arjun_v2
{
    internal class EmployeeProcess : BaseProcessor
    {
        private bool isValidFile { get; set; }

        private string EmployeeFilePath { get; set; }
        private string FailedReason { get; set; }
        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(EmployeeFilePath)); }
        }

        private List<EmployeeModel> employees;
        public EmployeeProcess(string Employeefile)

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
            employees = new List<EmployeeModel>();
            using (XmlReader reader = XmlReader.Create(EmployeeFilePath))
            {
                EmployeeModel empmodel = new EmployeeModel();

                int count = 0;
                while (reader.Read())
                {


                    if (reader.IsStartElement()) 
                    {
                        if (reader.Name.ToLower() == "employeecode")
                        {
                           
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
                            empmodel.WareHouseIdfk = reader.ReadElementContentAsString();
                            count++;

                            if (empmodel.WareHouseIdfk == "")
                            {
                                break;
                            }
                        }
                        else if (reader.Name.ToLower() == "contactnumber")

                        {
                            empmodel.ContactNumber = reader.ReadElementContentAsString();
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
                    if (count > 5 && empmodel.EmpCode != "")
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
                if (empdata.WareHouseIdfk != dirName)
                {
                    FailedReason = "invalid warehouse code ";
                    
                    return;

                }
            }
            isValidFile = true;

        }

        private void PushStoreDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
            using (SqlConnection conn = new SqlConnection(oMSConnectionString))
            {
                conn.Open();
                foreach (var data in employees)
                {
                    using (SqlCommand cmd = new SqlCommand($"if not  exists (select empname from Employee where empcode ='{data.EmpCode}') " +
                   $"begin " +
                   $"insert into Employee(empcode,EmpName,WareHouseIdfk,ContactNumber,Gender,Salary)" +
                   $"values ('{data.EmpCode}','{data.EmpName}',(select warehouseidpk from warehouse where warehousecode='{data.WareHouseIdfk}')," +
                   $"'{data.ContactNumber}'," +
                   $"'{data.Gender}','{data.Salary}')" +
                   $" end  " +

                   $"else " +

                   $" begin " +
                   $"update Employee  " +
                   $"set empcode='{data.EmpCode}',empname='{data.EmpName}', warehouseidfk=(select warehouseidpk from warehouse where warehousecode='{data.WareHouseIdfk}'), contactnumber='{data.ContactNumber}', gender='{data.Gender}',  Salary='{data.Salary}' " +
                   $"where  empcode='{data.EmpCode}'" +
                   $"end", conn))
                    {

                        cmd.ExecuteNonQuery();

                    }
                }

            }
        }
    }
}
