using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_IDAL
{
    public interface  IEmployeeDAL
    {
        bool EmployeeDBAcces(List<RSC_Models.EmployeeModel> empData, int storeid);
    }
}
