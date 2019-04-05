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
        private IHubProxy _userProfileHubProxy;

        private const string AngularUrl = "http://localhost:4200";
        private const string HubName = "userProfile";

        public MainWindow()
        {
            InitializeComponent();

            _hubConnection = new HubConnection(SignalRUri.Url);
            _userProfileHubProxy = _hubConnection.CreateHubProxy(HubName);

            _userProfileHubProxy.On("OnClientConnect", (clientId) =>
                Dispatcher.InvokeAsync(() =>
                {
                    OnAngularClientConnect(clientId);
                }));
            _userProfileHubProxy.On<dynamic>("OnClientDisconnected", data =>
                Dispatcher.InvokeAsync(() =>
                {
                    OnAngularClientDisconnected(data.stopCalled.Value, data.clientId.Value);
                }));
            _userProfileHubProxy.On("OnReceivedFromAngularClient", data =>
                Dispatcher.InvokeAsync(() =>
                {
                    OnReceivedFromAngularClient(data);
                }));

            _hubConnection.Start().Wait();
        }

        private void OnAngularClientConnect(string clientId)
        {
            AppendMessage($"Angular Client id:{clientId} connected.");

            var userProfileJson = "{\"FirstName\":\"Peter\", \"LastName\":\"Rabbit\"}";
            _userProfileHubProxy.Invoke("SendToAngularClient", clientId, userProfileJson);
            AppendMessage($"Sent user profile json: {userProfileJson} to Angular Client: {clientId}");
        }
        private void OnAngularClientDisconnected(bool stopCalled, string clientId)
        {
            if (stopCalled)
            {
                AppendMessage($"Angular Client {clientId} explicitly closed the connection. (e.g Called stop or closed the browser)");
            }
            else
            {
                AppendMessage($"WARNING: Angular Client {clientId} timed out!");
            }
        }
        private void OnReceivedFromAngularClient(string jsonBlob)
        {
            // Bring the window to the front
            this.Activate();
            AppendMessage($"Angular Client sent: {jsonBlob}");
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
