using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS_arjun;
using OMS_Arjun_V3.DataBaseAccessLayer;

namespace OMS_Arjun_V3.BusinessAccessLayer
{
    internal class WareHouseBAL:WareHouseDAL
    {
        private string WareHouseFilePath { get; set; }
        private string FailedReason { get; set; }
        private string dirName
        {
            get { return Path.GetFileName(Path.GetDirectoryName(WareHouseFilePath)); }
        }
        private string[] WareHouseFileContent { get; set; }
        private bool isValidFile { get; set; }
        private WareHouseModel wareHouseModel { get; set; }


        public WareHouseBAL(string WareHouseFile)

        {
            WareHouseFilePath = WareHouseFile;

        }

        internal void process()
        {
            ReadFileData();
            ValidateStoreData();
            PushWareHouseDataToDB();
        }

        private void ReadFileData()
        {

            WareHouseFileContent = File.ReadAllLines(WareHouseFilePath);

        }


        private void ValidateStoreData()
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
        private void PushWareHouseDataToDB()
        {
            if (!isValidFile)
            {
                return;
            }

            UpdateWareHouseDataToDB(WareHouseFileContent);            
        }
        


    }
}
