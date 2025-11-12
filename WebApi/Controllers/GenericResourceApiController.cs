using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class GenericResourceApiController<T> : GenericApiController<T>
    {
        private readonly IGenericService<T> _service;

        public GenericResourceApiController(IGenericService<T> service) : base(service)
        {
            _service = service;
        }

        [HttpGet]
        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            var entities = await _service.ReadAsync();
            //metoda pomocnicze Ok() - zwraca kod statusu 200 wraz z danymi
            return Ok(entities);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(T entity)
        {
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
