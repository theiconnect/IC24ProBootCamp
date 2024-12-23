using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTypes;
using FileHelper;
using Model;
using ConnectionConfig;

namespace OMS_IDataAccessLayer
{
    public interface IEmployeeDAL
    {
      void UpdateorinsertEmployeeDataToDB(string[] EmployeeFileContent, string EmployeeFilePath);
        
    }
}
