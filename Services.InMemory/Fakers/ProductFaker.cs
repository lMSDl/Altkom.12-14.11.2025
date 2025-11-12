namespace Services.InMemory.Fakers
{
    public class ProductFaker : EntityFaker<Models.Product>
    {
        public ProductFaker()
        {
            RuleFor(x => x.Name, f => f.Commerce.ProductName());
            RuleFor(x => x.Price, f => decimal.Parse(f.Commerce.Price()));
            RuleFor(x => x.Description, f => f.Commerce.ProductDescription());
            RuleFor(x => x.ShoppingListId, f => f.Random.Int(1, 10));
        }

    }
}
