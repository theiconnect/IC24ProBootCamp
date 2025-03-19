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
		public async Task<bool> AddSupplier(SupplierDTO DTO)
		{
			return await _supplierRepository.AddSupplier(DTO);

		}
		public  List<SupplierListDTO> GetSupplierList(SearchSupplierDTO searchDTO)
		{
			return  _supplierRepository.GetSupplierList(searchDTO);

		}
		public async Task<bool> EditSupplierDetails(SupplierEditDTO DTO)
		{
			return await _supplierRepository.EditSupplierDetails(DTO);

		}
		public async Task<bool> UpdateSupplierDetails(SupplierEditDTO DTO)
		{
			return await _supplierRepository.UpdateSupplierDetails(DTO);

		}
	}
}
