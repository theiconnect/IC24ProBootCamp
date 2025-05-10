using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        private readonly RscContext rscContext;
        public CustomerOrderController(RscContext context)
        {
            rscContext = context;

        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerOrders()
        {
            return Ok(await rscContext.Orders.ToListAsync());
        }
    }
}
