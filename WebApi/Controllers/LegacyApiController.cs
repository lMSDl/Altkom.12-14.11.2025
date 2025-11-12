using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Area("Legacy")]
    [Route("api/[area]/[controller]")]
    public abstract class LegacyApiController : ApiController
    {
    }
}
