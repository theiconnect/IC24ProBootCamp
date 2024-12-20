using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using FileModel;
using Configuration;
using ProjectHelpers;
using Enum;
using DBDataAcesses;
using OMSDAL;
using OMS_IDAL;
using OMSEntityDAL;
namespace FileProcesses
{
    public class WareHouseProcess : BaseProcessor
    {
        private string WareHouseFilePath{get;set;}
        private string FailedReason { get;set;}
        private string dirName {
            get { return Path.GetFileName(Path.GetDirectoryName(WareHouseFilePath)); } }
        private string[] WareHouseFileContent {  get;set;}
        private bool isValidFile {  get;set;}
        private WareHouseModel wareHouseModel { get;set;}   

        private IWareHouseDAL objDal { get;set;}

        public WareHouseProcess( string WareHouseFile, IWareHouseDAL wareHouseDAL) 
        
        {
            WareHouseFilePath = WareHouseFile;
            objDal = wareHouseDAL;
        }

        public void process()
        {
            ReadFileData();
            ValidateStoreData();

            if (!isValidFile)
            {
                FileHelper.MoiveFile(WareHouseFilePath, FileStatus.Failure);
                return;
            }

            bool isSuccess = objDal.PushWareHouseDataToDB(wareHouseModel);

            if (isSuccess)
            {
                FileHelper.MoiveFile(WareHouseFilePath, FileStatus.Success);
            }
            else
            {
                FileHelper.MoiveFile(WareHouseFilePath, FileStatus.Failure);
            }
        }

        private void ReadFileData()
        {
            WareHouseFileContent = File.ReadAllLines(WareHouseFilePath);
            prepareWareHouseObject();
        }


        private void ValidateStoreData()
        {
            if (WareHouseFileContent.Length > 2)
            { 
            
                FailedReason="Log: the file contain more than one store information";
            }

           else if(WareHouseFileContent.Length <= 1)
            {
                FailedReason = "Log: the file contain less than one store information";

            }

            else
            {
                string[] data = WareHouseFileContent[1].Split('|');


                if (data.Length!=5)
                {

                    FailedReason = "file contain more than 6 columns";
                }

               else if ( data[0].ToLower() != dirName.ToLower())
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
        
        private void prepareWareHouseObject()
        {
            wareHouseModel= new WareHouseModel();

            string[] data = WareHouseFileContent[1].Split('|');
            wareHouseModel.WareHouseCode = data[0];
            wareHouseModel.WareHouseName= data[1];
            wareHouseModel.Location = data[2];
            wareHouseModel.ManagerName= data[3];
            wareHouseModel.ContactNumber= data[4];
        }

        public   List<WareHouseModel> GetAllWareHouses()
        {
            return objDal.GetAllWareHouses();
        }

    }
}
