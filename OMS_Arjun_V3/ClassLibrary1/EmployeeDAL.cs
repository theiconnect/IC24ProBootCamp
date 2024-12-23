using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using ConnectionConfig;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using OMS_IDataAccessLayer;

namespace DataAccessLayer
{
    public class EmployeeDAL:ConnectionConfig1,IEmployeeDAL
    {
        

        public List<EmployeeModel> employees;

        public void UpdateorinsertEmployeeDataToDB(string[] EmployeeFileContent, string EmployeeFilePath)
        { 
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
