using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly RscContext rscContext;
        public StoreController(RscContext context)
        {
            rscContext= context;

        }
        [HttpGet]
        public async Task<IActionResult> GetStore()
        {
            return Ok(await rscContext.Stores.ToListAsync());

        }
    }
}
