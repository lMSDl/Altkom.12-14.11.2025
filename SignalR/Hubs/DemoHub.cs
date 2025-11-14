using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class DemoHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            Console.WriteLine(Context.ConnectionId);

            await Clients.Caller.SendAsync("alamakota", "Welcome to the DemoHub!");
            await Clients.All.SendAsync("PrintMessage", $"New member: {Context.ConnectionId}");
        }


        public async Task NotifyOthers(string message)
        {
            await Clients.Others.SendAsync("PrintMessage", message);
        }
        public async Task NotifyOthersInGroup(string message, string group)
        {
            await Clients.OthersInGroup(group).SendAsync("PrintMessage", message);
        }


        private static string connectionIdPlaceholder = "";
        public async Task Calculate(int a, int b)
        {
            //w tym przypadku Caller symuluje jakiegoś klienta który obliczy wartość (w naszym przypadku to ten sam co wysłał zapytanie)
            await Clients.Caller.SendAsync("Calculate", a, b);
            connectionIdPlaceholder = Context.ConnectionId;
        }

        public async Task CalculationResult(int x)
        {
            await Clients.Client(connectionIdPlaceholder).SendAsync("PrintMessage", x.ToString());
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("PrintMessage", $"{Context.ConnectionId} joined group {groupName}");
        }

    }
}
