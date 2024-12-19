using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionConfig
{
    public  class ConnectionConfig1
    {
        public static string rootFolderPath { get; set; }
        public static string ExcelConnString { get; set; }

        public static string oMSConnectionString { get; set; }

        static ConnectionConfig1()
        {

            rootFolderPath = ConfigurationManager.AppSettings["RootLocalFolder"];
            ExcelConnString = ConfigurationManager.AppSettings["ExcelConnectionString"];
            oMSConnectionString = ConfigurationManager.ConnectionStrings["iConnectOMSConnectionString"].ToString();
        }
    }
}
