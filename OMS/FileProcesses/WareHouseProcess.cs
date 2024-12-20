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


        public WareHouseProcess( string WareHouseFile) 
        
        {
            WareHouseFilePath = WareHouseFile;

        }

        public void process()
        {
            ReadFileData();
            ValidateStoreData();

            //IWareHouseDAL wareHouseDAL = new WarehouseDAL();
            IWareHouseDAL wareHouseDAL= new WareHouseEntityDAL();
            wareHouseDAL. PushWareHouseDataToDB(wareHouseModel,isValidFile, WareHouseFilePath);
        }

        private void ReadFileData()
        {

           WareHouseFileContent= File.ReadAllLines(WareHouseFilePath);
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


        

    }
}
