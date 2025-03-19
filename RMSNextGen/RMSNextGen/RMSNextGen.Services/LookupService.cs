using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Services
{
	public class LookupService
	{
		LookupRepository _lookupRepository;
		public LookupService(LookupRepository lookupRepository)
		{
            _lookupRepository = lookupRepository;
		}
		public async Task<List<DepartmentDto>> GetDepartments()
		{
			return await _lookupRepository.GetDepartments();
		}

	}
}
