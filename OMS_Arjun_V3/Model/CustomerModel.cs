using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    public class CustomerModel
    {
        public string CustomerIdpk { get; set; }
        public int CustomerName { get; set; }
        public string PhNo { get; set; }
        public List<OrdersModel> Orders { get; set; }

        public List<WareHouseModel> WareHouseCode { get; set; }

       
    }
}
