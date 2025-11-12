using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers
{
    public class ShoppingListsController : ApiController
    {
        private readonly IList<Models.ShoppingList> _shoppingLists;

        public ShoppingListsController(IList<Models.ShoppingList> shoppingLists)
        {
            _shoppingLists = shoppingLists;
        }


        //Aby móc zastosować metody pomocnicze REST używamy jako typu zwracanego IActionResult, ActionResult lub ActionResult<T>
        public ActionResult<IEnumerable<ShoppingList>> Get()
        {
            //metoda pomocnicze Ok() - zwraca kod statusu 200 wraz z danymi
            return Ok(_shoppingLists);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ShoppingList> GetById(int id)
        {
            var entity= _shoppingLists.FirstOrDefault(x => x.Id == id);

            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public ActionResult<int> Post(Models.ShoppingList shoppingList)
        {
            shoppingList.Id = _shoppingLists.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _shoppingLists.Add(shoppingList);

            //zamiast ręcznie konfigurować odpowiedź HTTP, używamy metody pomocniczej CreatedAtAction()
            //HttpContext.Response.StatusCode = 201; //Created

            /*return CreatedAtAction(
                nameof(GetById),
                new { id = shoppingList.Id },
                shoppingList);*/ //tutaj zwracamy cały obiekt

            //metoda CreatedAtAction pozwala na wskazanie funkcji, która może zostać użyta do pobrania nowo utworzonego zasobu
            return CreatedAtAction(
                nameof(GetById),
                new { id = shoppingList.Id }, //obiekt anonimowy z parametrami trasy - nazwa "id" to nazwa parametru z metody GetById
                shoppingList.Id); //tutaj zwracamy tylko Id nowo utworzonego zasobu
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Models.ShoppingList shoppingList)
        {
            var index = _shoppingLists.ToList().FindIndex(x => x.Id == id);
            if (index == -1)
            {
                return NotFound();
            }

            _shoppingLists[index] = shoppingList;

            return NoContent(); //204 No Content
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var entity = _shoppingLists.FirstOrDefault(x => x.Id == id);
            if (entity is null)
                return NotFound();


            _shoppingLists.Remove(entity);

            return NoContent(); //204 No Content
        }
    }
}
