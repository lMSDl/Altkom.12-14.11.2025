
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ServiceFilter<LimiterFilter>]
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

        public override async Task<ActionResult<int>> Post(Person entity)
        {
            var people = await _peopleService.ReadByName(entity.FullName);
            if(people.Any())
            {
                ModelState.AddModelError("FullName", $"Person with the name '{entity.FullName}' already exists.");
                //return Conflict($"Person with the name '{entity.FullName}' already exists.");
            }

            //jeśli zawiesimy automatyczną walidację modelu, to możemy sprawdzić poprawność modelu ręcznie
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await base.Post(entity);
        }
    }
}
