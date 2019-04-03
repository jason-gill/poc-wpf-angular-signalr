using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Owin.Hosting;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Start SignalR here
            Task.Run(() => this.StartWebServer());

            // Create the startup window
            MainWindow wnd = new MainWindow {Title = "Wpf App"};
            wnd.Show();
        }

        private void StartWebServer()
        {
            var url = SignalRUri.Url;

            WebApp.Start<WebStartupConfig>(url);
            Debug.WriteLine($"Server started at: {url}");
        }
    }
}
