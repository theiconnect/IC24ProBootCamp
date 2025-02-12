﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Models
{
    internal class StoreModel
    {
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public String StoreName { get; set; }
        public string Location { get; set; }
        public string ManagerName { get; set; }
        public string ContactNumber { get; set; }
    }
}
