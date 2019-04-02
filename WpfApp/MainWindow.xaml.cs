using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public MainWindow()
        {
            InitializeComponent();

            _hubConnection = new HubConnection("http://localhost:9013/");
            _contractHubProxy = _hubConnection.CreateHubProxy("contract");

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
            Process.Start("http://localhost:4200/contract");
        }
    }
}
