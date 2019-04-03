using System.Net;
using System.Net.Sockets;
using System.Security.Policy;

namespace WpfApp
{
    public static class SignalRUri
    {
        static SignalRUri()
        {
            Host = "localhost";
            Port = GetRandomUnusedPort();
            Url = $"http://{Host}:{Port}/";
        }

        public static string Host { get; }

        public static int Port { get; }

        public static string Url { get; }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}