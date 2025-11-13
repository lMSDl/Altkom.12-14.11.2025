using Microsoft.AspNetCore.Mvc;
using Models;
using Services.InMemory;
using Services.InMemory.Fakers;
using Services.Interfaces;
using FluentValidation;
using WebApi.Validators;
using WebApi.Filters;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    /*.AddJsonOptions(options =>
    {
        //konfigurujemy serializacje aby obslugiwala cykle referencji za pomoc¹ mechanizmu referencji ($id, $ref)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        //ignorowanie wlasciwosci tylko do odczytu podczas serializacji
        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        //ignorowanie wartosci domyslnych podczas serializacji
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
    });*/
    .AddNewtonsoftJson(options =>
    {
        //konfigurujemy serializacje aby obslugiwala cykle referencji
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
        options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
    })
    .AddXmlDataContractSerializerFormatters(); //ws³¹czamy wsparcie dla formatu XML

builder.Services.AddSingleton<IList<int>>([.. Enumerable.Range(1, 100).Select(x => Random.Shared.Next())]);
/*builder.Services.AddSingleton<IList<ShoppingList>>([
    new() { Id = 1, Name = "Groceries" },
    new(){ Id = 2, Name = "Electronics" },
    new(){ Id = 3, Name = "Clothing" }
]);*/
builder.Services.AddSingleton<IGenericService<ShoppingList>, GenericService<ShoppingList>>();
builder.Services.AddSingleton<IPeopleService, PeopleService>();
//builder.Services.AddSingleton<IGenericService<Product>, GenericService<Product>>();
builder.Services.AddSingleton<IGenericService<Product>>(x => new GenericService<Product>(x.GetRequiredService<Bogus.Faker<Product>>(), 
                                                                                         x.GetRequiredService<IConfiguration>().GetValue<int>("Bogus:NumberOfNestedResources")));
builder.Services.AddTransient<Bogus.Faker<Person>, PersonFaker>();

//builder.Services.AddTransient<Bogus.Faker<Product>, ProductFaker>();
builder.Services.AddTransient<Bogus.Faker<Product>>(x => new ProductFaker(x.GetRequiredService<IConfiguration>()["Bogus:Language"]!,
                                                                          x.GetRequiredService<IConfiguration>().GetValue<int>("Bogus:NumberOfResources")));

builder.Services.AddTransient<Bogus.Faker<ShoppingList>, ShoppingListFaker>();
builder.Services.AddTransient(x => x.GetRequiredService<IConfiguration>().GetSection("Bogus").Get < Models.Settings.Bogus>()!);

/*builder.Services.Configure<Models.Settings.Bogus>(builder.Configuration.GetSection("Bogus"));*/
//walidacja ustawien
builder.Services.AddOptions<Models.Settings.Bogus>()
    .Bind(builder.Configuration.GetSection("Bogus"))
    .ValidateDataAnnotations()
    .Validate(x => !string.IsNullOrWhiteSpace(x.Language), "No language defined")
    .ValidateOnStart();

//zawieszenie automatycznej walidacji modelu
builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter  = true);

//podejscie legacy (dla wersji FluentValudation < 12)
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<ShoppingList>, ShoppingListValidator>();

builder.Services.AddSingleton<ConsoleLogFilter>();
builder.Services.AddSingleton(new LimiterFilter(5));

builder.Services.AddResponseCompression(x =>
{
    x.Providers.Clear();
    x.Providers.Add<BrotliCompressionProvider>();
    x.Providers.Add<GzipCompressionProvider>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();


app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Headers.AcceptLanguage.Any())
    {
        var lang = context.Request.Headers.AcceptLanguage.ToString().Split(',').FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(lang))
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo(lang);
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo(lang);
        }
    }

    await next(context);
});

app.MapControllers();

app.Run();
