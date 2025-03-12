﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDataAccess;
using Models;
using EntityDataAccess.EF;
namespace EntityDataAccess
{
    
    public class StoreEntityDA:IStoreDA
    {
         public RscEntities RSCDB { get; set; }
        public StoreEntityDA()
        {
            RSCDB = new RscEntities();
        }
        public  List<StoreModel> GetAllStoresDataFromDB()
        {
            List < StoreModel > model=new List<StoreModel> ();
            var  stores=RSCDB.Stores.ToList();
            foreach (var store in stores)
            {
                StoreModel st = new StoreModel();
                st.StoreName= store.StoreName;
                st.StoreIdPk=store.StoreIdPk;
                st.StoreCode = store.StoreCode;
                st.ContactNumber=store.StoreContactNumber; 
                st.ManagerName=store.ManagerName;
                st.Location=store.Location;
                model.Add(st);
            }
            return model;
        }
        public bool SyncStoreDataToDB(StoreModel storeModelObject)
        {
            var dbStore=RSCDB.Stores.FirstOrDefault(s=>s.StoreCode== storeModelObject.StoreCode);
            if(dbStore == null)
            {
                Console.WriteLine($"file Store code:{storeModelObject.StoreCode} didn't match with the db records.");
            }
            else
            {
                dbStore.StoreName= storeModelObject.StoreName;
                dbStore.StoreContactNumber = storeModelObject.ContactNumber;
                dbStore.ManagerName= storeModelObject.ManagerName;
                dbStore.Location= storeModelObject.Location;
                RSCDB.SaveChanges();
            }
            return true;
        }
    }
}
