﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StoreModel
    {
        //Model or BO or DTO are same 
        

        public int StoreIdPk { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string ManagerName { get; set; }
        public string ContactNumber { get; set; }

    }
}
