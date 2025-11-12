
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class PeopleController : GenericApiController<Person>
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService service) : base(service)
        {
            _peopleService = service;
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
    }
}
