using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly RscContext rscContext;
        public EmployeeController(RscContext context)
        {
            rscContext= context;

        }
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await rscContext.Employees.ToListAsync());
        }
    }
}
