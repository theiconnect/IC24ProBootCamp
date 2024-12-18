using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_Models
{
    public class StoreModel
    {
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }

        public string ManagerName { get; set; }
        public List<StoreModel> storemodels { get; set; }
        public string ContactNumber { get; set; }
    }
}
