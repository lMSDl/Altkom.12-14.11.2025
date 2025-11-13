using System.IO.Pipelines;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.UseOwin(pipe => pipe(dic => OwinResponse));

app.Run();




async Task OwinResponse(IDictionary<string, object> dictionary)
{
    string response = "Hello in OWIN World!";

    var body = (Stream)dictionary["owin.ResponseBody"];
    var headers = (IDictionary<string, string[]>)dictionary["owin.ResponseHeaders"];

    byte[] bytes= System.Text.Encoding.UTF8.GetBytes(response);
    headers["Content-Length"] = new[] { bytes.Length.ToString() };
    headers["Content-Type"] = new[] { "text/plain" };
    await body.WriteAsync(bytes, 0, bytes.Length);
}
