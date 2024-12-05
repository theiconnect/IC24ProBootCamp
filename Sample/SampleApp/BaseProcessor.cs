using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    internal class BaseProcessor
    {
        protected static string rSCConnectionString { get; set; }

        static BaseProcessor()
        {
            rSCConnectionString = ConfigurationManager.ConnectionStrings["iConnectRSCConnectionString"].ToString();
        }
    }
}
