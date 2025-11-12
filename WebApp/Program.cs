var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//middleware poœrednicz¹cy
app.Use(async (httpContext, next) =>
{
    Console.WriteLine(httpContext.GetEndpoint()?.DisplayName);
    Console.WriteLine("Before Use1");
    await next(httpContext);
    Console.WriteLine("After Use1");
});

//rêczne w³¹czenie routingu - jeœli nie jest jawnie w³¹czone, to jest automatycznie u¿yte na pocz¹tku pipeline'u
//app.UseRouting();

app.Use(async (httpContext, next) =>
{
    Console.WriteLine(httpContext.GetEndpoint()?.DisplayName);
    Console.WriteLine("Before Use2");
    await next();
    Console.WriteLine("After Use2");
});


//rêczne w³¹czenie endpointów - jeœli nie jest jawnie w³¹czone, to jest automatycznie u¿yte na koñcu pipeline'u
//pe³na forma definiowania endpointów
//app.UseEndpoints(x => x.MapGet("/", () => "Hello World!"));

//middleware terminuj¹cy
app.Run(async (httpContext) =>
{
    Console.WriteLine("hello");
   await httpContext.Response.WriteAsync("Not found!");
});

//skrót do definiowania endpointów
app.MapGet("/", () => "Hello World!");



app.Run();
