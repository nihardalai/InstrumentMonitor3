using System.Windows;

namespace InstrumentMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        BootStrapper _bootStrapper;

        public App()
        {
            _bootStrapper = new BootStrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _bootStrapper.Run();
        }
    }
}
