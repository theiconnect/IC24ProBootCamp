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
        
        
        void SyncEmployeeDataWithDB(List<EmployeeDTO> fileEmployeeDTOObject, int storeIdFk);
    }
}
