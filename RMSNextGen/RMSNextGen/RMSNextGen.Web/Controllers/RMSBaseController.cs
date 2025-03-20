using Microsoft.AspNetCore.Mvc;

namespace RMSNextGen.Web.Controllers
{
    public class RMSBaseController : Controller
    {
        public RMSBaseController()
        {
            UserName = "iConnectAdmin";
        }

        public string UserName { get;set; }
    }
}
