using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WpfApp
{
    [HubName("contract")]
    public class ContractHub : BaseHub
    {
        public void Save(string contractJson)
        {
            Debug.WriteLine($"Contract: {contractJson}");
        }
   }

    public class BaseHub : Hub
    {
        public override Task OnConnected()
        {
            Debug.WriteLine("Client connected: " + Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Debug.WriteLine("Client disconnected: " + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}