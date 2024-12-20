using FileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_IDAL
{
    public interface ICustomerDAL
    {
         void PushCustomerDataToDB(List<CustomerModel> Customers, int wareHouseId, string customerFilePath, string ordersFilePath, string orderItemFilePath);

        void PushOrderDataToDB(List<CustomerModel> Customers, int wareHouseId, string ordersFilePath, string orderItemFilePath);

        void PushOrderItemDataToDB(List<CustomerModel> Customers, int wareHouseId, string orderItemFilePath);
        

    }
}
