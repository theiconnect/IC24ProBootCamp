using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;
using ConnectionConfig;


namespace BusinessAccessLayer
{
    public class WareHouseBAL
    {
        public string WareHouseFilePath { get; set; }
        public string FailedReason { get; set; }
        public string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(WareHouseFilePath)); }
        }
        public string[] WareHouseFileContent { get; set; }
        public bool isValidFile { get; set; }
        public WareHouseModel wareHouseModel { get; set; }     



        public WareHouseBAL(string WareHouseFile)

        {
            WareHouseFilePath = WareHouseFile;

        }

        public void process()
        {
            ReadFileData();
            ValidateWareHouseData();
            PushWareHouseDataToDB();

        }
       

        public void ReadFileData()
        {

            WareHouseFileContent = File.ReadAllLines(WareHouseFilePath);

        }


        public void ValidateWareHouseData()
        {
            if (WareHouseFileContent.Length > 2)
            {

                FailedReason = "Log: the file contain more than one store information";
            }

            else if (WareHouseFileContent.Length <= 1)
            {
                FailedReason = "Log: the file contain less than one store information";

            }

            else
            {
                string[] data = WareHouseFileContent[1].Split('|');


                if (data.Length != 5)
                {

                    FailedReason = "file contain more than 6 columns";
                }

                else if (data[0].ToLower() != dirName.ToLower())
                {
                    FailedReason = "warehosue code doesn't match";
                }

            }

            if (!string.IsNullOrEmpty(FailedReason))
            {
                //log this error message into a file
                return;
            }
            isValidFile = true;


        }
        public void PushWareHouseDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }

           new WareHouseDAL().UpdateWareHouseDataToDB(WareHouseFileContent, WareHouseFilePath);
        }
        


    }
}
