using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] //ukrywanie kontrolera albo posczególnych metod w OpenApi
    public class AdminController : ApiController
    {
        [HttpGet]
        public ActionResult<string> GetSecret()
        {
            return "This is a secret message";
        }
    }
}
