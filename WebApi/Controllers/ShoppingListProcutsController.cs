using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/ShoppingLists/{parentId:int}/Products")]
    public class ShoppingListProcutsController : GenericNestedResourcesApiController<Product, ShoppingList>
    {
        public ShoppingListProcutsController(IGenericService<Product> service, IGenericService<ShoppingList> parentService) : base(service, parentService)
        {
        }

        protected override ActionResult<int> CreatedAtAction(int id)
        {
            return CreatedAtAction(
                nameof(GenericApiController<Product>.GetById),
                "Products",
                new { id = id},
                id);
        }

        protected override int GetParentId(Product item)
        {
            return item.ShoppingListId;
        }

        protected override int SetParentId(Product item, int id)
        {
            item.ShoppingListId = id;
            return id;
        }
    }
}
