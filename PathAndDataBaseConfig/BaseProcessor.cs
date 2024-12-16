﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathAndDataBaseConfig
{
    
    public class BaseProcessor
    {
        public static string mainFolderPath { get; set; }
        public static string excelConnectionString { get; set; }
        protected static string rscConnectedString { get; set; }


        public BaseProcessor()
        {
            mainFolderPath = ConfigurationManager.AppSettings["RootFolder"];

            rscConnectedString = ConfigurationManager.ConnectionStrings["iConnectRSCConnectionString"].ToString();


            excelConnectionString = ConfigurationManager.AppSettings["ExcelConnectionString"].ToString();

        }
    }
}