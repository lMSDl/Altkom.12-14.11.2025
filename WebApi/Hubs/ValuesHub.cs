using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class ValuesHub : Hub
    {
        private readonly IList<int> _list;

        public ValuesHub(IList<int> list)
        {
            _list = list;
        }

        public Task Get()
        {
            return Clients.Caller.SendAsync("ReceiveValues", _list);
        }
    }
}
