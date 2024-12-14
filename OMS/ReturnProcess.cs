using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMS
{
    internal class ReturnProcess: BaseProcessor
    {
        private string ReturnFilePath {  get; set; }
        private int WareHouseId {  get; set; }
        private List<ReturnsModel> ReturnsList {get; set;}

        public ReturnProcess(string returnFilePath, int wareHouseidpk)
        {
            this.ReturnFilePath = returnFilePath;
            this.WareHouseId = wareHouseidpk;
        }

        internal void Process()
        {
            if (string.IsNullOrEmpty(ReturnFilePath))

            {
                Console.WriteLine($"WareHouse Id :-{WareHouseId} Return file  is missing");
                return;
            }
            ReadFileData();
            ValidateReturnsData();
            PushRetrunsDataToDB();

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

        private void PushRetrunsDataToDB()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(DBHelper.oMSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "AddReturn";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        foreach (var returnRecord in ReturnsList)
                        {
                            if (!returnRecord.IsvalidReturn) continue;
                            cmd.Parameters.Clear();

                            // Add parameters with correct DbType and values
                            cmd.Parameters.Add(new SqlParameter("@ReturnDate", SqlDbType.DateTime) { Value = returnRecord.Date });
                            cmd.Parameters.Add(new SqlParameter("@InvoiceNumber", SqlDbType.NVarChar, 50) { Value = returnRecord.InvoiceNumber });
                            cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.NVarChar, 255) { Value = returnRecord.Reason });
                            cmd.Parameters.Add(new SqlParameter("@ReturnStatus", SqlDbType.NVarChar, 50) { Value = returnRecord.ReturnStatus });
                            cmd.Parameters.Add(new SqlParameter("@AmountRefund", SqlDbType.Decimal) { Value = returnRecord.AmountRefund });

                            // Execute the stored procedure
                            cmd.ExecuteNonQuery();

                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}