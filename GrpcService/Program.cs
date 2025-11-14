using GrpcService.Services;
using Services.InMemory.Fakers;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSingleton<IPeopleService, Services.InMemory.PeopleService>();
builder.Services.AddTransient<Bogus.Faker<Models.Person>, PersonFaker>();

builder.Services.AddTransient(x => x.GetRequiredService<IConfiguration>().GetSection("Bogus").Get<Models.Settings.Bogus>()!);
builder.Services.Configure<Models.Settings.Bogus>(builder.Configuration.GetSection("Bogus"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<PeopleService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
