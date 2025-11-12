using Models;
using Services.InMemory;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IList<int>>([.. Enumerable.Range(1, 100).Select(x => Random.Shared.Next())]);
/*builder.Services.AddSingleton<IList<ShoppingList>>([
    new() { Id = 1, Name = "Groceries" },
    new(){ Id = 2, Name = "Electronics" },
    new(){ Id = 3, Name = "Clothing" }
]);*/
builder.Services.AddSingleton<IGenericService<ShoppingList>, GenericService<ShoppingList>>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
