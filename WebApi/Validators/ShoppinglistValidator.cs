using FluentValidation;
using Models;
using Services.Interfaces;

namespace WebApi.Validators
{
    public class ShoppingListValidator : AbstractValidator<ShoppingList>
    {

        public ShoppingListValidator(IGenericService<ShoppingList> service) {

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)/*.WithMessage("ala ma kota")*/
                .Must(name => name.Contains("Zakupy")).WithMessage("Nazwa musi zawierać słowo \"Zakupy\"")
                .WithName("Nazwa");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now)
                .WithName("Utworzono");

            RuleFor(x => x)
                .MustAsync(async (item, cancelationToken) =>
                {
                    var existingItems = await service.ReadAsync();
                    return !existingItems.Any(x => x.Name == item.Name);
                })
                .WithMessage("An item with the same name already exists!");
        }
    }
}
