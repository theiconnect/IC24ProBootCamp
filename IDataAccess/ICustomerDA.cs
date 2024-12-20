using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface ICustomerDA
    {
        void SyncCustomerData(List<CustomerModel> customers);
        void SyncCustomerDataWithDB(List<CustomerModel> customers);
        void SyncOrderDataWithDB(List<CustomerModel> customers);
        void SyncBillingDataWithDB(List<CustomerModel> customers);


    }
}
