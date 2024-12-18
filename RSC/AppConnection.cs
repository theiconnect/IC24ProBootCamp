using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC.AppConnection_Kiran
{
    public class AppConnection
    {
        public static string rootFolderPath { get; set; }
        public static string rSCConnectionString { get; set; }

        static AppConnection()
        {
            rootFolderPath = ConfigurationManager.AppSettings["RootFolder"];
            rSCConnectionString = ConfigurationManager.ConnectionStrings["kiranRSCConnectionString"].ToString();
        }
    }
}
        

