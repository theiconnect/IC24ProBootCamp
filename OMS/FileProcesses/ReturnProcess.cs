using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FileProcesses;
using FileModel;
using Configuration;
using ProjectHelpers;
using System.Globalization;
using DBDataAcesses;
using OMS_IDAL;
using OMSEntityDAL;

namespace FileProcesses
{
    public class ReturnProcess: BaseProcessor
    {
        private string ReturnFilePath {  get; set; }
        private int WareHouseId {  get; set; }
        private List<ReturnsModel> ReturnsList {get; set;}
        private IReturnsDAL objReturnsDal {  get; set; }
        public ReturnProcess(string returnFilePath, int wareHouseidpk, IReturnsDAL objReturnsDal)
        {
            this.ReturnFilePath = returnFilePath;
            this.WareHouseId = wareHouseidpk;
            this.objReturnsDal = objReturnsDal;
        }

        public void Process()
        {
            if (string.IsNullOrEmpty(ReturnFilePath))

            {
                Console.WriteLine($"WareHouse Id :-{WareHouseId} Return file  is missing");
                return;
            }
            ReadFileData();
            ValidateReturnsData();
           

            bool isSucess=objReturnsDal.PushReturnsDataToDB(ReturnsList,ReturnFilePath);
            if (isSucess)
            {
                FileHelper.MoiveFile(ReturnFilePath, Enum.FileStatus.Success);

            }
            else
            {
                FileHelper.MoiveFile(ReturnFilePath, Enum.FileStatus.Failure);
            }

        }

        private void ReadFileData()
        {
            
            DataSet dsReturn=FileHelper.GetXMLFileContent(ReturnFilePath);

            PrepareReturns(dsReturn);

        }

        private void PrepareReturns(DataSet dsReturn)
        {
            ReturnsList = new List<ReturnsModel>();
           foreach (DataRow row in dsReturn.Tables[0].Rows)
            {
                ReturnsModel returnModel = new ReturnsModel();
                returnModel.InvoiceNumber = Convert.ToString(row["InvoiceNo"]);
                returnModel.WareHouseCode = Convert.ToString(row["WareHouseCode"]);
                returnModel.ReturnStatus = Convert.ToString(row["ReturnStatus"]);
                returnModel.Reason = Convert.ToString(row["Reason"]);
                if (DateTime.TryParse(Convert.ToString( row["ReturnDate"]),out DateTime result))
                {
                    returnModel.Date = result.ToString("yyyy-MM-dd");
                }
                if (decimal.TryParse(Convert.ToString(row["AmountRefund"]), out decimal TotalAmount))
                {
                    returnModel.AmountRefund = TotalAmount;
                }
                else
                {
                    returnModel.IsvalidReturn = false;
                }
                ReturnsList.Add(returnModel);
            }

        }

        private void ValidateReturnsData()
        {
            foreach (var returnRecord in ReturnsList)
            {
                if (!returnRecord.IsvalidReturn) continue;
                if (returnRecord.InvoiceNumber == string.Empty)
                {
                    returnRecord.IsvalidReturn = false;
                    continue;
                }
            }
        }

       


    }
}