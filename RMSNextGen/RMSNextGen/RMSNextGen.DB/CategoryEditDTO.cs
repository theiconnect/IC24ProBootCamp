﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
    public class CategoryEditDTO
    {
        public int ProductCategoryIdPk { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
