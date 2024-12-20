using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IEmployeeDA
    {
        void SyncEmployeeData(List<EmployeeDTO> employeeData, List<EmployeeDTO> fileEmployeeDTOObject);

         void GetAllEmployeeDataFromDB(List<EmployeeDTO> employeeData);
        void SyncEmployeeDataWithDB(List<EmployeeDTO> fileEmployeeDTOObject);
    }
}
