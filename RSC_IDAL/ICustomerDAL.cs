using RSC_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_IDAL
{
    public interface ICustomerDAL
    {
         void CustomerDBAcces(List<CustumerModel> custumerData, int storeid);
        void CustomerOrderPushToDB(List<CustumerModel> custumerData);
        bool pushOrderBillingDataToDB(List<CustumerModel> custumerData);
    }
}
