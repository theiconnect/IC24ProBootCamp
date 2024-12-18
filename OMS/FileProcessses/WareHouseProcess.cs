﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Configuration;
using FileModel;
using OMS.DataAccessLayer_Muni;


namespace FileProcessses
{
    public class WareHouseProcess :DBHelper
    {
       
        private WareHouseModel wareHouseModel { get; set; }
        public string WareHouseFilePath{get;set;}
        private string FailedReason { get;set;}
        private string dirName {
            get { return Path.GetFileName(Path.GetDirectoryName(WareHouseFilePath)); } }
        public string[] WareHouseFileContent {  get;set;}
        private bool isValidFile {  get;set;}
        public static List<WareHouseModel> wareHouses { get { return GetWareHousesDataFromDb.getAllWareHouses(); } }



        public WareHouseProcess( string WareHouseFile) 
        
        {
            WareHouseFilePath = WareHouseFile;

        }
       

        public void process()
        {
            ReadFileData();
            ValidateStoreData();
            if (!isValidFile)
            {
                Console.WriteLine("Log the error:not a valid file");
                return;
            }
            SyncWareHouseDataToDB.PushWareHouseDataToDB(wareHouseModel);
        }

        private void ReadFileData()
        {

           WareHouseFileContent= File.ReadAllLines(WareHouseFilePath);

        }
        public void prepareWareHouseObject()
        {
            wareHouseModel = new WareHouseModel();

            string[] data = WareHouseFileContent[1].Split('|');
            wareHouseModel.WareHouseCode = data[0];
            wareHouseModel.WareHouseName = data[1];
            wareHouseModel.Location = data[2];
            wareHouseModel.ManagerName = data[3];
            wareHouseModel.ContactNumber = data[4];
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
       
        
        //read file


        //validate file
        //push file

        

    }
}
