using System.Windows;

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

            // Create the startup window
            MainWindow wnd = new MainWindow {Title = "Wpf App"};
            wnd.Show();
        }
    }
}
