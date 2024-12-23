using FileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.IDataAccessLayer_Muni
{
    public interface IEmployeeDAL
    {
         void PushEmployeeDataToDB(List<EmployeeModel> employees);
    }
}
