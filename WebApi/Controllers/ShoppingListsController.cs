using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ShoppingListsController : ApiController
    {
        private readonly IList<Models.ShoppingList> _shoppingLists;

        public ShoppingListsController(IList<Models.ShoppingList> shoppingLists)
        {
            _shoppingLists = shoppingLists;
        }


        public IEnumerable<Models.ShoppingList> Get()
        {
            return _shoppingLists;
        }

        [HttpGet("{id:int}")]
        public Models.ShoppingList? GetById(int id)
        {
            return _shoppingLists.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public void Post(Models.ShoppingList shoppingList)
        {
            shoppingList.Id = _shoppingLists.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _shoppingLists.Add(shoppingList);
        }

        [HttpPut("{id:int}")]
        public void Put(int id, Models.ShoppingList shoppingList)
        {
            var index = _shoppingLists.ToList().FindIndex(x => x.Id == id);
            if (index != -1)
            {
                _shoppingLists[index] = shoppingList;
            }
        }
    }
}
