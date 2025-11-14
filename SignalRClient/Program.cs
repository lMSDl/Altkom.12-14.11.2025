using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

IList<int> ints = [];

var signalR = new HubConnectionBuilder()
    .WithUrl("https://localhost:5017/signalR/values")
    .Build();

signalR.On<int>(nameof(Delete), Delete);
signalR.On<int>(nameof(Add), Add);
signalR.On<IEnumerable<int>>(nameof(Get), Get);


await signalR.StartAsync();
await signalR.SendAsync("Get");

while(signalR.State == HubConnectionState.Connected)
{
    Console.Clear();
    Console.WriteLine(string.Join(", ", ints));
    Console.ReadLine();
}


void Delete(int x)
{
    ints.Remove(x);
}

void Add(int x)
{
    ints.Add(x);
}

void Get(IEnumerable<int> enumerables)
{
    ints = enumerables.ToList();
}



Console.ReadLine();

static async Task DemoHub()
{
    var signalR = new HubConnectionBuilder()
        .WithUrl("https://localhost:7023/SignalR/demo")
        .WithAutomaticReconnect()
        .Build();

    signalR.Reconnecting += async (exception) =>
    {
        Console.WriteLine("Reconnecting...");
        await Task.CompletedTask;
    };

    signalR.Reconnected += async (connectionId) =>
    {
        Console.WriteLine("Reconnected to the server.");
        await Task.CompletedTask;
    };

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

        if (splitted.Length == 2)
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
}