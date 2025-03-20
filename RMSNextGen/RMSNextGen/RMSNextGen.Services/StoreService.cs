using Microsoft.Identity.Client;
using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Services
{
	public class StoreService
	{
		StoreRepository _storeRepository;
		public StoreService(StoreRepository addStore)
		{
			_storeRepository= addStore;
		}

		

		public async Task <bool> AddStore(AddStoreDTO Objectdto)
		{
			return await _storeRepository.AddStore(Objectdto);
		}

		public   List<StoreListDTO> GetStores()
		{
			return  _storeRepository.GetStores();
		}

		public List<StateDTO> GetStates()
		{
			return _storeRepository.GetStates();
		}

		public List<CityDTO> GetCities( int stateId)
		{
			return _storeRepository.GetCities(stateId);
				
		}

		//public List<SearchStoresDTO> SearchStores(SearchStoresDTO searchdto)
		//{
		//	 return _storeRepository.SearchStores( SearchStores);
		//}


	}
	

}
