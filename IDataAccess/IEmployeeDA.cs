﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IEmployeeDA
    {
        
        
        bool SyncEmployeeDataWithDB(List<EmployeeDTO> fileEmployeeDTOObject);
    }
}
