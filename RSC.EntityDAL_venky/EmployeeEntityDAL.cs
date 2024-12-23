using RSC.Configuration_Venky;
using RSC.FileModels_Venky;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.IDAL;

namespace RSC.EntityDAL_venky
{
    internal class EmployeeEntityDAL : AppConnection, IEmployeeDAl
    {
        
            public void PushStoreDataToDB(EmployeeModel model)
            {

               
            }
        
    }
}
