using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_Arjun_V3
{
    internal class ConnectionConfig
    {
        protected static string rootFolderPath { get; set; }
        protected static string ExcelConnString { get; set; }

        protected static string oMSConnectionString { get; set; }

        static ConnectionConfig()
        {

            rootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            ExcelConnString = ConfigurationManager.AppSettings["ExcelConnectionString"];
            oMSConnectionString = ConfigurationManager.ConnectionStrings["iConnectOMSConnectionString"].ToString();
        }
    }
}
