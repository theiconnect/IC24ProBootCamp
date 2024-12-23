using RSC.FileModel_Kiran;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.AppConnection_Kiran;
using RSC_IDAL;
using System.Reflection;

namespace DataBaseAccessLayer
{
    public class SyncEmployeePushToDB:AppConnection,IEmployeeDAL
    {
        private int StoreId {  get; set; }  
        public void PushStoreDataToDB(List<EmployeeModel>Employees, int storeId)
        {
            this.StoreId = storeId; 
           
            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                string query = $"Update Employees Set =EmployeeCode=@EmployeeCode,EmployeeName=@EmployeeName,EmployeeRole=@EmployeeRole,EmployeeDateOfJoining=@EmployeeDateOfJoining,EmployeeDateOfLeaving=@employeeDateOfleaving,EmployeeContactNumber=@EmployeeContactNumber,EmployeeGender=@EmployeeGender,EmployeeSalary=@EmployeeSalary,StoreidPk=@storeid";
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    foreach (EmployeeModel model in Employees)
                    {
                        cmd.Parameters.Add("@EmployeeCode", DbType.String).Value = model.employeeCode;
                        cmd.Parameters.Add("@EmployeeName", DbType.String).Value = model.employeeName;
                        cmd.Parameters.Add("@EmployeeRole", DbType.String).Value = model.employeeRole;
                        cmd.Parameters.Add("@EmployeeDateOfJoining", DbType.String).Value = model.employeeDateOfJoining;
                        cmd.Parameters.Add("@EmployeeDateOfleaving", DbType.String).Value = model.employeeDateOfLeaving;
                        cmd.Parameters.Add("@EmployeeContactNumber", DbType.String).Value = model.employeeContactNumber;
                        cmd.Parameters.Add("@EmployeeGender", DbType.String).Value = model.employeeGender;
                        cmd.Parameters.Add("@EmployeeSalary", DbType.String).Value = model.employeeSalary;
                        cmd.Parameters.Add("@storeid", DbType.String).Value = this.StoreId;
                        //Employees.add(model);
                        con.Close();
                    }
                       

                }
            }
        }





    }
}
