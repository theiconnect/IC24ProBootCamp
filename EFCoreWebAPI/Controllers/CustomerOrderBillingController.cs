using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderBillingController : ControllerBase
    {
        private readonly RscContext rscContext;
        public CustomerOrderBillingController(RscContext context)
        {
            rscContext = context;

        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerOrderBilling()
        {
            return Ok(await rscContext.Billings.ToListAsync());
        }
    }
}
