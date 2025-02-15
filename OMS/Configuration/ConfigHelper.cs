using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration
{
    public class ConfigHelper
    {
        protected static string RootFolderPath { get; set; }
        protected static string ExcelConnString { get; set; }
        protected static bool UseEf { get; set; }
        static ConfigHelper()
        {

            UseEf = Convert.ToBoolean(ConfigurationManager.AppSettings["UseEf"]);
            RootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            ExcelConnString = ConfigurationManager.AppSettings["ExcelConnectionString"];
        }
    }

    public   class DBHelper
    {
        protected static string oMSConnectionString { get; set; }
                               
        static DBHelper()
        {
            oMSConnectionString = ConfigurationManager.ConnectionStrings["iConnectOMSConnectionString"].ToString();
        }
    }
}
