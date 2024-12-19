using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionConfig
{
    internal class Program
    {
        protected static string rootFolderPath { get; set; }
        protected static string ExcelConnString { get; set; }

        protected static string oMSConnectionString { get; set; }
        static void Main(string[] args)
        {
            rootFolderPath = ConfigurationManager.AppSettings["RootLocalFolder"];
            ExcelConnString = ConfigurationManager.AppSettings["ExcelConnectionString"];
            oMSConnectionString = ConfigurationManager.ConnectionStrings["iConnectOMSConnectionString"].ToString();

        
        }
    }
}
