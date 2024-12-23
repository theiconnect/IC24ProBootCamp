using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_IDAL
{
    public interface IemployeeDA
    {
         bool SyncEmployeeWithDB(List<EmployeeModel> empData, int storeid);
    }
}
