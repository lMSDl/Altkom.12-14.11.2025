using HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddSqlServer("Data Source=(local);Initial Catalog=HealthChecks;Integrated Security=True;TrustServerCertificate=true")
    .AddCheck(nameof(DirectoryAccessHealth), new DirectoryAccessHealth { DirectoryPath = @"C:\\temp"});
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();
