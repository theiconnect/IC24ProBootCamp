using FileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.IDataAccessLayer_Muni.IEmployeeDAL
{
    internal interface ISyncEmployeeDataToDB
    {
         void PushEmployeeDataToDB(List<EmployeeModel> employees);
    }
}
