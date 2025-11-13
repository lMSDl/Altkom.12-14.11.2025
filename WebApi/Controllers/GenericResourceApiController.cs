using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    public class GenericResourceApiController<T> : GenericApiController<T>
    {
        private readonly IGenericService<T> _service;

        public GenericResourceApiController(IGenericService<T> service, IValidator<T>? validator = null) : base(service, validator)
        {
            _service = service;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            var entities = await _service.ReadAsync();
            //metoda pomocnicze Ok() - zwraca kod statusu 200 wraz z danymi
            return Ok(entities);
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public virtual async Task<ActionResult<int>> Post(T entity)
        {
            if (_validator is not null)
            {
                var result = await _validator.ValidateAsync(entity);

                if (!result.IsValid)
                {
                    return BadRequest(result.ToDictionary());
                }
            }


            var id = await _service.CreateAsync(entity);

            //zamiast ręcznie konfigurować odpowiedź HTTP, używamy metody pomocniczej CreatedAtAction()
            //HttpContext.Response.StatusCode = 201; //Created

            /*return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                shoppingList);*/ //tutaj zwracamy cały obiekt

            //metoda CreatedAtAction pozwala na wskazanie funkcji, która może zostać użyta do pobrania nowo utworzonego zasobu
            return CreatedAtAction(
                nameof(GetById),
                new { id = id }, //obiekt anonimowy z parametrami trasy - nazwa "id" to nazwa parametru z metody GetById
                id); //tutaj zwracamy tylko Id nowo utworzonego zasobu
        }
    }
}
