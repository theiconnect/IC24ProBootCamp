﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface ICustomerDA
    {
        bool SyncCustomerOrderData(List<CustomerModel> customers);
        

        

    }
}
