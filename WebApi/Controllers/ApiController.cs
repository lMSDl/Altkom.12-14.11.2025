using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController] // oznaczamy nasz kontroler jako API
    [Route("api/[controller]")] //adres naszego kontrolera - w nawiasach kwadratowych nazwa klasy bez "Controller"
    public abstract class ApiController : ControllerBase
    {
    }
}
