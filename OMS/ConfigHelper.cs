using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    internal class ConfigHelper
    {
        protected static string RootFolderPath { get; set; }
        protected static string ExcelConnString { get; set; }
        static ConfigHelper()
        {

            RootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
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
