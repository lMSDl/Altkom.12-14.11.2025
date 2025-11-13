using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class ShoppingListsController : GenericResourceApiController<ShoppingList>
    {
        public ShoppingListsController(IGenericService<ShoppingList> service, IValidator<ShoppingList> validator) : base(service, validator)
        {
        }

        /*[NonAction] //wyłącza tę metodę z routingu API
        public override Task<ActionResult> Put(int id, ShoppingList entity)
        {
            return base.Put(id, entity);
        }*/
    }
}
