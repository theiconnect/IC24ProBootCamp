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
            //return Ok(await rscContext.Employees.ToListAsync());
            //above one or below same
            var data = await rscContext.Employees.ToListAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeBYId(int id) 
        {
            var employee=await rscContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        //[HttpPost]
        //public async Task<ActionResult> CreateEmployee(EmployeeModel emp)
        //{
                        
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var employeeObj = new Employee();

        //    employeeObj.EmployeeCode = emp.EmployeeCode;
        //    employeeObj.StoreCode = emp.StoreCode;
        //    employeeObj.EmployeeName = emp.EmployeeName;
        //    employeeObj.Role = emp.Role;
        //    employeeObj.DateOfJoining = Convert.ToDateTime(emp.DateOfJoining);
        //    employeeObj.DateOfLeaving = emp.DateOfLeaving;
        //    employeeObj.ContactNumber = emp.ContactNumber;
        //    employeeObj.Gender = emp.Gender;
        //    employeeObj.Salary = emp.Salary;
        //    employeeObj.StoreIdFk = emp.StoreIdFk;
            
        //    await rscContext.Employees.AllAsync(employeeObj);
        //    await rscContext.SaveChangesAsync();
        //    return Ok(employeeObj);



        //}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id,Employee emp)
        {
            if (id != emp.EmployeeIdPk) 
            {
                return BadRequest();
            }
            rscContext.Entry(emp).State=EntityState.Modified;
            await rscContext.SaveChangesAsync();
            return Ok(emp);

        }
    }
}
