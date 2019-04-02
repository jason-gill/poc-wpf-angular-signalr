using System.Diagnostics;
using System.Dynamic;
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
            Clients.Others.OnDataFromClient(contractJson);
        }

        public void SendToClient(string clientId, string contractName)
        {
            Clients.Client(clientId).dataFromServer(contractName);
        }
   }

    public class BaseHub : Hub
    {
        public override Task OnConnected()
        {
            Clients.Others.OnClientConnect(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            dynamic data = new ExpandoObject();
            data.stopCalled = stopCalled;
            data.clientId = Context.ConnectionId;

            Clients.Others.OnClientDisConnected(data);
            return base.OnDisconnected(stopCalled);
        }
    }
}