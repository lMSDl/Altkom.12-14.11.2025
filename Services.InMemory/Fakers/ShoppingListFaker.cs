using Models;

namespace Services.InMemory.Fakers
{
    public class ShoppingListFaker : EntityFaker<ShoppingList>
    {
        public ShoppingListFaker()
        {
            RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);
            RuleFor(x => x.CreatedAt, x => x.Date.Past(1));
        }
    }
}
