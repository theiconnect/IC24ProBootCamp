using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.AppConnection_Kiran;
using RSC_Validations;
using RSC.FileModel_Kiran;
using System.IO;
using RSC_IDAL;
using DataBaseAccessLayer;

namespace BusinessAccessLayer
{
    public class StockProcesser 
    {
        private string stockFilePath { get; set; }
        private int storeid { get; set; }
        private string Storecode { get; set; }
        private string[] StockFileContant { get; set; }
        public IStockDAL StockObj { get; set; }
        public List<StockModel> stocks { get; set; }

        public StockProcesser(string StockFilePath, int Storeid, string storeDirName, IStockDAL StockDALobj)
        {
            StockObj= StockDALobj;
           stockFilePath = StockFilePath;
            storeid = storeid;
            Storecode = storeDirName;
        }
        public void Processor()
        {
            ReadFileData();

        }

        private void ReadFileData()
        {
            StockFileContant = File.ReadAllLines(stockFilePath);
            StockValidations obj = new StockValidations();
            obj.ValidateDate(StockFileContant, storeid);
        }        
    }
}


           