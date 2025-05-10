using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly RscContext rscContext;
        public StockController(RscContext context) 
        {
            rscContext= context;

        }
        [HttpGet]
        public async Task<IActionResult> GetStock()
        {
            return Ok(await rscContext.Stocks.ToListAsync());
        }
    }
}
