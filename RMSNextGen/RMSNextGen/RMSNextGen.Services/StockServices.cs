using RMSNextGen.DAL;
using RMSNextGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Services
{
    public class StockServices
    {
		StockRepository _stockRepository;
		public StockServices(StockRepository stockRepository)
		{
			_stockRepository = stockRepository;
		}
		public async Task<bool> SaveStock(StockDTO stockObj)
		{
			return await _stockRepository.SaveStock(stockObj);

		}
	}
}
