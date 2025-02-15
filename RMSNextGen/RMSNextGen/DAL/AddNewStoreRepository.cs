using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.DAL
{
	internal class AddNewStoreRepository
	{
		public string connectionstring;	

		public AddNewStoreRepository(string connectionstring)
		{
			this.connectionstring = connectionstring;
		}
		
		public async  Task <bool> AddStore(AddNewStoreDTO objectdto)
		{
			using SqlConnection conn = new SqlConnection(connectionstring)
		  {
			  await connection.OpenAsync();
		  }
		}
		
	}
}
