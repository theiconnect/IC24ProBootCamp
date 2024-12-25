using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataBaseConfig;
using RSC_IDAL;
using System.Data;
using System.Runtime.Remoting.Messaging;
namespace RSC_EntityDAL

{
    public class EmployeeprocessorDA : IemployeeDA
    {
        public bool SyncEmployeeWithDB(List<EmployeeModel> empData, int Storeid)
        {

            return true;
        }
    }
}