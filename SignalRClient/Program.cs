using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

var signalR = new HubConnectionBuilder()
    .WithUrl("https://localhost:7023/SignalR/demo")
    .Build();

signalR.On<string>(nameof(PrintMessage), PrintMessage);
signalR.On<string>("alamakota", PrintMessage);
signalR.On<int, int>(nameof(Calculate), Calculate);


await signalR.StartAsync();

await signalR.SendAsync("NotifyOthers", "Hello from SignalR Client!");

await signalR.SendAsync("Calculate", 2, 7);

while (signalR.State == HubConnectionState.Connected)
{
    var message = Console.ReadLine();
    var splitted = message.Split(" & ");

    if(splitted.Length == 2)
    {
        await signalR.SendAsync(splitted[0], splitted[1]);
    }
    else if (splitted.Length == 3)
    {
        await signalR.SendAsync(splitted[0], splitted[1], splitted[2]);
    }
}




void PrintMessage(string message)
{
    Console.WriteLine($"Received message: {message}");
}

async Task Calculate(int a, int b)
{
    var result = a + b; // Example calculation
    await signalR.SendAsync("CalculationResult", result);
}



Console.ReadLine();