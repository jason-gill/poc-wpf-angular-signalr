using System.Diagnostics;
using System.Windows;
using Microsoft.AspNet.SignalR.Client;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _hubConnection;
        private IHubProxy _contractHubProxy;

        private const string AngularUrl = "http://localhost:4200";
        private const string HubName = "contract";

        public MainWindow()
        {
            InitializeComponent();

            _hubConnection = new HubConnection(SignalRUri.Url);
            _contractHubProxy = _hubConnection.CreateHubProxy(HubName);

            _contractHubProxy.On("OnClientConnect", (clientId) =>
                Dispatcher.InvokeAsync(() =>
                {
                    OnClientConnect(clientId);
                }));
            _contractHubProxy.On<dynamic>("OnClientDisconnected", data =>
                Dispatcher.InvokeAsync(() =>
                {
                    OnClientDisconnected(data.stopCalled.Value, data.clientId.Value);
                }));
            _contractHubProxy.On("OnDataFromClient", data =>
                Dispatcher.InvokeAsync(() =>
                {
                    OnDataFromClient(data);
                }));

            _hubConnection.Start().Wait();
        }

        private void OnClientConnect(string clientId)
        {
            AppendMessage($"Client connected: {clientId}");

            var contractName = "<Contract Name>";
            _contractHubProxy.Invoke("SendToClient", clientId, contractName);
            AppendMessage($"Sent contract name: {contractName} to client: {clientId}");
        }
        private void OnClientDisconnected(bool stopCalled, string clientId)
        {
            if (stopCalled)
            {
                AppendMessage($"Client {clientId} explicitly closed the connection. (e.g Called stop or closed the browser)");
            }
            else
            {
                AppendMessage($"WARNING: Client {clientId} timed out!");
            }
        }
        private void OnDataFromClient(string contractJson)
        {
            // Bring the window to the front
            this.Activate();
            AppendMessage($"Client sent: {contractJson}");
        }

        private void ClearMessages()
        {
            MessagesListView.Items?.Clear();
        }

        private void AppendMessage(string message)
        {
            MessagesListView.Items?.Add(message);
        }

        private void LaunchWebAppButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start($"{AngularUrl}/{HubName}?signalRUrl={SignalRUri.Url}");
        }
    }
}
