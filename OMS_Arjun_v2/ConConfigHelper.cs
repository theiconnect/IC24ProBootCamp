using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_Arjun_v2
{
    internal class ConConfigHelper
    {
        protected static string rootFolderPath { get; set; }
        protected static string ExcelConnString { get; set; }
        static ConConfigHelper()
        {

            rootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            ExcelConnString = ConfigurationManager.AppSettings["ExcelConnectionString"];
        }
    }
    internal class DBHelper
    {
        protected static string oMSConnectionString { get; set; }

        static DBHelper()
        {
            oMSConnectionString = ConfigurationManager.ConnectionStrings["iConnectOMSConnectionString"].ToString();
        }
    }
}
