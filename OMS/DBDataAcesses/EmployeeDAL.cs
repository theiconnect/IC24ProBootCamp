using Enum;
using FileModel;
using ProjectHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Configuration;
using OMS_IDAL;
using System.Linq;

namespace DBDataAcesses
{
    public class EmployeeDAL:BaseProcessor,IEmployeeyDAL
    {
        public   bool PushEmployeeDataToDB(List<EmployeeModel> EmployeesList)
        {
            SqlConnection conn=null;
            try
            {
                using ( conn = new SqlConnection(oMSConnectionString))
                {
                    

                    using (SqlCommand cmd = new SqlCommand("InsertOrUpdateEmpData", conn))
                    {
                        conn.Open();
                        var count = 0;
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
                            count++;
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        FileHelper.LogEntries($"[{DateTime.Now}] INFO: The Employee file which is  associated with the warehouse code {EmployeesList.Select(x => x.EmpWareHouseCode).FirstOrDefault()} is successfully processed  and the file is moved to processed folder. Record got affected:{count}\n");
                    }

                }
                
                return true;
            }

            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                Console.WriteLine(ex.Message);
                FileHelper.LogEntries($"[{DateTime.Now}] ERROR: The Employee file which is  associated with the warehouse code {EmployeesList.Select(x => x.EmpWareHouseCode).FirstOrDefault()} is not a valid file.got an error '{ex.Message}' and  the file is moved to error folder. Please check and update the file \n");
                return false;
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

    }
}
