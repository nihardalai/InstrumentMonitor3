using Microsoft.Practices.ServiceLocation;
using Ninject;
using System.Windows;

namespace InstrumentMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var kernel = ServiceLocator.Current.GetInstance<IKernel>();
            var vm = kernel.Get<MainWindowViewModel>();
            this.DataContext = vm;
        }
    }
}
