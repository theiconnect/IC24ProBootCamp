using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseConfig
{
    public class ConfigHelper
    {
        public static string rootFolderPath { get; set; }
        public static string connectionString { get; set; }
        static ConfigHelper()
        {

            rootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            connectionString = ConfigurationManager.AppSettings["iConnectRSCConnectionString"];
        }
    }

    
}