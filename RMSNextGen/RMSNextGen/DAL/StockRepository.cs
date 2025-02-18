using Microsoft.Data.SqlClient;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.DAL
{
	public class StockRepository
	{
		public readonly string _connectionString;

		public StockRepository(string connectionString)
		{
			_connectionString = connectionString;
		}
		public async Task<bool> SaveStock(StockDTO stockObj)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (SqlCommand command = new SqlCommand())
				{
					command.CommandText = "Insert Into Stock(StockCode,PurchaseOrderNumber,InvoiceNumber,StockInTime,Remarks,VehicleNumber,ApprovedBy,ApprovedOn,ApprovedComments,CreatedBy,CreatedOn)" +
						"values(@StockCode,@PurchaseOrderNumber,@InvoiceNumber,@StockInTime,@Remarks,@VehicleNumber,@ApprovedBy,GetDate(),@ApprovedComments,@CreatedBy,GetDate())";
					command.Connection = connection;
					try
					{
						command.Parameters.AddWithValue("@StockCode", stockObj.StockCode);
						command.Parameters.AddWithValue("@PurchaseOrderNumber", stockObj.PurchaseOrderNumber);
						command.Parameters.AddWithValue("@InvoiceNumber", stockObj.InvoiceNumber);
						command.Parameters.AddWithValue("@StockInTime", stockObj.StockInTime);
						command.Parameters.AddWithValue("@Remarks", stockObj.Remarks);
						command.Parameters.AddWithValue("@VehicleNumber", stockObj.VehicleNumber);
						command.Parameters.AddWithValue("@ApprovedBy", stockObj.ApprovedBy);
						command.Parameters.AddWithValue("@ApprovedComments", stockObj.ApprovedComments);
						command.Parameters.AddWithValue("@CreatedBy", stockObj.CreatedBy);


						await command.ExecuteNonQueryAsync();
						return true;

					}
					catch (Exception ex)
					{
						return false;
					}
					finally
					{
						connection.Close();
					}

				}

			}

		}
	}
}
