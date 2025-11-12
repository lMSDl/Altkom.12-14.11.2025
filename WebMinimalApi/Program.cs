using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IList<int>>([.. Enumerable.Range(1, 100).Select(x => Random.Shared.Next())]);

var app = builder.Build();

//minimalna API - bez kontrolerów
//mo¿emy dodaæ adnotacje do metod anonimowych np. [Authorize]
//parametry wyra¿enia lambda s¹ wstrzykiwane z kontenera DI lub z requestu (httpContextu)
app.MapGet("/values", /*[Authorize]*/ (IList<int> values) => values );
//{value:int} - parametr w œcie¿ce z okreœlonym typem
app.MapDelete("/values/{value:int}", (IList<int> values, int value) => values.Remove(value) );
app.MapPost("/values/{value:int}", (IList<int> values, int value) => values.Add(value) );
//newValue - parametr z cia³a lub query requestu
//mo¿emy dodaæ atrybut [FromBody] lub [FromQuery] aby okreœliæ sk¹d ma byæ pobrany parametr
app.MapPut("/values/{oldValue:int}", (IList<int> values, int oldValue, /*[FromQuery]*/ int newValue) => values[values.IndexOf(oldValue)] = newValue);


app.Run();
