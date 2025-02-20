using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Services
{
    public class SupplierEditService
    {
		SupplierEditRepository _supplierEditRepository;
		public SupplierEditService(SupplierEditRepository supplierEditRepository)
		{
			_supplierEditRepository = supplierEditRepository;
		}
		public async Task<bool> EditSupplier(SupplierEditDTO DTO)
		{
			return await _supplierEditRepository.EditSupplierDetails(DTO);
		}
	}
}
