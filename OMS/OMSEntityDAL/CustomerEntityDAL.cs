using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileModel;
using OMS_IDAL;

namespace OMSEntityDAL
{
    public class CustomerEntityDAL: ICustomerDAL
    {
        public  void PushCustomerDataToDB(List<CustomerModel> Customers, int wareHouseId, string customerFilePath, string ordersFilePath, string orderItemFilePath)
        {
            

            PushOrderDataToDB(Customers, wareHouseId, ordersFilePath, orderItemFilePath);

        }
        public  void PushOrderDataToDB(List<CustomerModel> Customers, int wareHouseId, string ordersFilePath, string orderItemFilePath)
        {
            
            PushOrderItemDataToDB(Customers, wareHouseId, orderItemFilePath);

        }
        public  void PushOrderItemDataToDB(List<CustomerModel> Customers, int wareHouseId, string orderItemFilePath)
        {
            

        }

    }
}
