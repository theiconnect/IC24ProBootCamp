using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RSC_Configurations
{
    public class AppConfiguration
    {
        public static string mainFolderPath { get; set; }
        public static string dbConnectionstring { get; set; }
        public static string XlsxConnectionstring { get; set; }


        static AppConfiguration()
        {
            mainFolderPath = ConfigurationManager.AppSettings["rootfolder"];
            dbConnectionstring = ConfigurationManager.ConnectionStrings["iconnectrscconnectionstring"].ToString();
            XlsxConnectionstring = ConfigurationManager.ConnectionStrings["Xlsxconnectionstring"].ToString();

        }
    }
}
