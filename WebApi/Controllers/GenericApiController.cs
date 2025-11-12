using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class GenericApiController<T> : ApiController
    {
        private readonly IGenericService<T> _service;

        public GenericApiController(IGenericService<T> service)
        {
            _service = service;
        }

        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public async Task<ActionResult<IEnumerable<T>>> Get()
        {
            var entities = await _service.ReadAsync();
            //metoda pomocnicze Ok() - zwraca kod statusu 200 wraz z danymi
            return Ok(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var entity = await _service.ReadByIdAsync(id);

            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
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

        [HttpPut("{id:int}")]
        public virtual async Task<ActionResult> Put(int id, T entity)
        {
            var localEntity = await _service.ReadByIdAsync(id);
            if (localEntity is null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, entity);

            return NoContent(); //204 No Content
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _service.ReadByIdAsync(id);
            if (entity is null)
                return NotFound();

            await _service.DeleteAsync(id);

            return NoContent(); //204 No Content
        }
    }
}
