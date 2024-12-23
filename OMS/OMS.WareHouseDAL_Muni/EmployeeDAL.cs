using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;
using Configuration;
using OMS.IDataAccessLayer_Muni;

namespace OMS.DataAccessLayer_Muni
{
    public class EmployeeDAL:DBHelper,IEmployeeDAL
    {
        public  void PushEmployeeDataToDB(List<EmployeeModel> employees)
        {
            SqlConnection conn = null;
            try
            {

                using (conn = new SqlConnection(oMSConnectionString))
                {
                    conn.Open();
                    foreach (var data in employees)
                    {
                        using (SqlCommand cmd = new SqlCommand("InsertOrUpdateEmpData", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@EmpCode", DbType.String).Value = data.EmpCode;
                            cmd.Parameters.Add("@EmpName", DbType.String).Value = data.EmpName;
                            cmd.Parameters.Add("@EmpWareHouseCode", DbType.String).Value = data.EmpWareHouseCode;
                            cmd.Parameters.Add("@EmpContactNumber", DbType.String).Value = data.empContactNumber;
                            cmd.Parameters.Add("@Gender", DbType.String).Value = data.Gender;
                            cmd.Parameters.Add("@Salary", DbType.Decimal).Value = data.Salary;

                            cmd.ExecuteNonQuery();

                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

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
