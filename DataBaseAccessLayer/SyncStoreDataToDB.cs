﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSC.FileModel_Kiran;
using RSC.AppConnection_Kiran;
using RSC_IDAL;

namespace DataBaseAccessLayer
{
    public class SyncStoreDataToDB:AppConnection,IStoreDAL
    {
        public void PushStoreDataToDB(List<StoreModel> Stores)
        {
            using (SqlConnection con = new SqlConnection(rSCConnectionString))
            {
                string query = $"Update Stores Set StoreName = @StoreName, Location = @Location, ManagerName= @Manager, ContactNumber = @ContactNumber where StoreCode = @StoreCode";
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    foreach (StoreModel Model in Stores)
                    {
                        cmd.Parameters.Add("@StoreName", DbType.String).Value = Model.storeName;
                        cmd.Parameters.Add("@StoreCode", DbType.String).Value = Model.storeCode;
                        cmd.Parameters.Add("@Location", DbType.String).Value = Model.location;
                        cmd.Parameters.Add("@Manager", DbType.String).Value = Model.managerName;
                        cmd.Parameters.Add("@ContactNumber", DbType.String).Value = Model.contactNumber;
                        con.Close();
                    }
                }
            }

        }
    }
}
