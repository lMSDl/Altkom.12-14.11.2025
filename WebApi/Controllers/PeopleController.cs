
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class PeopleController : GenericResourceApiController<Person>
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService service) : base(service)
        {
            _peopleService = service;
        }

        [NonAction]
        public override Task<ActionResult<IEnumerable<Person>>> Get()
        {
            return base.Get();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetByName(string? name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return await Get();
            }

            var people = await _peopleService.ReadByName(name);
            return Ok(people);
        }

        // Produces - wymusza format odpowiedzi bez względu na nagłówek Accept w żądaniu
        //[Produces("application/xml")]
        public override Task<ActionResult<Person>> GetById(int id)
        {
            return base.GetById(id);
        }
    }
}
