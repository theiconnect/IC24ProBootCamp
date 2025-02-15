using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMSNextGen.DAL;
using RMSNextGen.Models;

namespace RMSNextGen.Services
{
	public class SupplierService
	{
		SupplierRepository _supplierRepository;
		public SupplierService(SupplierRepository supplierRepository)
		{
			_supplierRepository = supplierRepository;
		}
		public async Task<bool> SaveStudent(SupplierDTO DTO)
		{
			return await _supplierRepository.SaveSupplier(DTO);

		}
	}
}
