using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WpfApp
{
    [HubName("userProfile")]
    public class UserProfileHub : BaseHub
    {
        /// <summary>
        /// Used by the Angular Client to send data to the WpfApp 
        /// </summary>
        /// <param name="jsonBlob"></param>
        public void SendToWpfApp(string jsonBlob)
        {
            // The WpfApp should subscribe to 'OnReceivedFromAngularClient'
            Clients.Others.OnReceivedFromAngularClient(jsonBlob);
        }

        /// <summary>
        /// Used by the WpfApp to send data to the Angular Client 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="jsonBlob"></param>
        public void SendToAngularClient(string clientId, string jsonBlob)
        {
            // The Angular Client should subscribe to 'OnReceivedFromServer' 
            Clients.Client(clientId).OnReceivedFromWpfApp(jsonBlob);
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