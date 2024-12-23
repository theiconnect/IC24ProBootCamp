using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTypes;
using FileHelper;
using Model;
using ConnectionConfig;
using OMS_IDataAccessLayer;
using EnityDataAccessLayer.EntityFramework;


namespace OMSEnityDataAccessLayer
{
    public class EnityEmployeeDAL:IEmployeeDAL
    { 
        public Arjun_OMSDBEntities1 Arjun_OMSDBEntities1 { get; set; }
        public EnityEmployeeDAL() 
        {
            Arjun_OMSDBEntities1=new Arjun_OMSDBEntities1();
        }
        

        public void UpdateorinsertEmployeeDataToDB(string[] EmployeeFileContent, string EmployeeFilePath)
        {

        }
    }
}
