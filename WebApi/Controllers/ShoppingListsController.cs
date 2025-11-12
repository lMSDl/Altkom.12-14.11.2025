using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class ShoppingListsController : ApiController
    {
        private readonly IGenericService<ShoppingList> _service;

        public ShoppingListsController(IGenericService<ShoppingList> service)
        {
            _service = service;
        }


        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public async Task<ActionResult<IEnumerable<ShoppingList>>> Get()
        {
            var entities = await _service.ReadAsync();
            //metoda pomocnicze Ok() - zwraca kod statusu 200 wraz z danymi
            return Ok(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShoppingList>> GetById(int id)
        {
            var entity= await _service.ReadByIdAsync(id);

            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Models.ShoppingList shoppingList)
        {
            var id = await _service.CreateAsync(shoppingList);

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
        public async Task<ActionResult> Put(int id, Models.ShoppingList shoppingList)
        {
            var entity = await _service.ReadByIdAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            
            await _service.UpdateAsync(id, shoppingList);

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
