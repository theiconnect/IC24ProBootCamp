using RSC_Configurations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC_IDAL;


namespace RSC_DataAccess
{
    public class EmployeeDBAccess:IEmployeeDAL
    {
        private int Storeid { get; set; }
        public  bool EmployeeDBAcces(List<RSC_Models.EmployeeModel> empData, int storeid)
        {
            this.Storeid = storeid;
            try
            {
                using (SqlConnection con = new SqlConnection(AppConfiguration.dbConnectionstring))
                {
                    con.Open();
                    string StoreProcedure = "employeeDataToDB";
                    using (SqlCommand cmd = new SqlCommand(StoreProcedure, con))
                    {
                        foreach (var employeedata in empData)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@empcode", employeedata.EmpCode);
                            cmd.Parameters.Add("@employeename", employeedata.EmployeeName);
                            cmd.Parameters.Add("@role", employeedata.Role);
                            cmd.Parameters.Add("@dateofjoining", employeedata.DateOfJoiningstr);
                            cmd.Parameters.Add("@dateofleaving", employeedata.DateOfLeavingstr);
                            cmd.Parameters.Add("@contactnumber", employeedata.ContactNumber);
                            cmd.Parameters.Add("@gender", employeedata.Gender);
                            cmd.Parameters.Add("@salary", employeedata.Salary);
                            cmd.Parameters.Add("@storeidfk", this.Storeid);
                        }

                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
    }
}