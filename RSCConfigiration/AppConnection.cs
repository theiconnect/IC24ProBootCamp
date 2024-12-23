using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSCConfigiration
{
    public class AppConnection
    {
        public static string rootFolder { get; set; }

        public static string rSCConnectionString { get; set; }
        static AppConnection()
        {
            rootFolder = ConfigurationManager.AppSettings["RootFolder"];
            rSCConnectionString = ConfigurationManager.ConnectionStrings["iconnectRSCConnectionString"].ToString();
        }

    }
}
