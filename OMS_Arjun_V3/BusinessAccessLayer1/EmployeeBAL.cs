using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;
using ConnectionConfig;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace BusinessAccessLayer1
{
    public class EmployeeBAL
    {
        public bool isValidFile { get; set; }

        public string EmployeeFilePath { get; set; }
        public string FailedReason { get; set; }
        public string[] EmployeeFileContent {  get; set; }
        public string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(EmployeeFilePath)); }
        }

        public List<EmployeeModel> employees;
        public EmployeeBAL(string Employeefile)

        {

            EmployeeFilePath = Employeefile;
        }

        public void Process()
        {

            //READ
            //VALIDATE
            //PUSH INTO DB
            ReadFileData();
            ValidateEmployeeData();
            PushEmployeeDataToDB();


        }

        public void ReadFileData()
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

        public void ValidateEmployeeData()
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

        public void PushEmployeeDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }
           
            new EmployeeDAL().UpdateorinsertEmployeeDataToDB(EmployeeFileContent, EmployeeFilePath);

        }
    }
}
