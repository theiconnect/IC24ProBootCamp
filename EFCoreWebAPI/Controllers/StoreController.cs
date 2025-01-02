using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(int id)
        {
            return Ok( rscContext.Stores.FindAsync(id));

        }
        [HttpPost]
        public async Task<ActionResult> CreateStore(StoreModel store)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Store storeObj = new Store();
           
            storeObj.StoreCode = store.StoreCode;
            storeObj.StoreName = store.StoreName;
            storeObj.Location = store.Location;
            storeObj.ManagerName = store.ManagerName;
            storeObj.StoreContactNumber=store.ContactNumber;
            
            


            
            await rscContext.Stores.AddAsync(storeObj);
            await rscContext.SaveChangesAsync();
            return Ok(storeObj);



        }
    }
}
