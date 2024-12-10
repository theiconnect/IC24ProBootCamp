using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    internal class WareHouseProcess : BaseProcessor
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

        internal void process()
        {
            ReadFileData();
            ValidateStoreData();
            PushStoreDataToDB();
        }

        private void ReadFileData()
        {

           WareHouseFileContent= File.ReadAllLines(WareHouseFilePath);

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
        private void PushStoreDataToDB()
        {
            if (!isValidFile)
            {
                return ;
            }

            prepareWareHouseObject();

            using(SqlConnection conn=new SqlConnection(oMSConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update warehouse Set warehouseName = '{wareHouseModel.WareHouseName}', Location = '{wareHouseModel.Location}', ManagerName= '{wareHouseModel.ManagerName}', ContactNo = '{wareHouseModel.ContactNumber}' where WareHouseCode = '{wareHouseModel.WareHouseCode}'", conn))
                {

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
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

        //read file


        //validate file
        //push file

        public static List<WareHouseModel> getAllWareHouses()
        {
            var wareHouses = new List<WareHouseModel>();
            SqlConnection i = null;
            try
            {
                using (i = new SqlConnection(oMSConnectionString))
                {

                    using (var cmd = new SqlCommand(DBConstants.GET_ALL_WAREHOUSES, i))
                    {
                        i.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var wareHouse = new WareHouseModel();
                                wareHouse.WareHouseidpk = Convert.ToInt32(reader["WarehouseIdPk"]);
                                wareHouse.WareHouseCode = Convert.ToString(reader["WarehouseCode"]);
                                wareHouse.WareHouseName = Convert.ToString(reader["WarehouseName"]);
                                wareHouse.Location = Convert.ToString(reader["Location"]);
                                wareHouse.ManagerName = Convert.ToString(reader["ManagerName"]);
                                wareHouse.ContactNumber = Convert.ToString(reader["ContactNo"]);
                                wareHouses.Add(wareHouse);
                            }
                        }
                        if (i.State == ConnectionState.Open)
                        {
                            i.Close();
                        }
                    }
                    return wareHouses;
                }
            }
            catch (Exception ex)
            {
                if (i.State == ConnectionState.Open)
                {
                    i.Close();
                }
                
            }
            finally
            {
                if (i != null && i.State == ConnectionState.Open)
                {
                    i.Close();
                }
                i.Dispose();
            }
            return wareHouses;
        }

    }
}
