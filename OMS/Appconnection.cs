using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_saikumar
{
    internal class Appconnection
    {
       public  static string baseFolderPath {  get; set; }
         public static string dbConnectionString { get; set; }
     
        static Appconnection()
        {
            baseFolderPath = ConfigurationManager.AppSettings["rootfolder"];
            dbConnectionString = ConfigurationManager.ConnectionStrings
                ["iconnectrscconnectionstring"].ToString();
        }
    }
    
}
