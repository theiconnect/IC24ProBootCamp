using EFCoreWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly RscContext rscContext;
        public CustomerController(RscContext context) 
        {
            rscContext = context;

        }
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            return Ok(await rscContext.Customers.ToListAsync());

        }
    }
}
