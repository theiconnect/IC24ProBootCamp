using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC.FileModel_Kiran 
{ 
    public class StoreModel
    {
        public int storeId { get; set; }
        public  string storeCode { get; set; }
        public  string storeName { get; set; }
        public  string location { get; set; }
        public  string managerName { get; set; }
        public  string contactNumber { get; set; }
        public List<StoreModel> storeModels { get; set; }

    }
}
