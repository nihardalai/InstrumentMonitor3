using MarketDataService;
using Ninject;
using Prism.Ninject;

namespace InstrumentMonitor
{
    public class BootStrapper : NinjectBootstrapper
    {
        protected override void ConfigureKernel()
        {
            base.ConfigureKernel();

            Kernel.Bind<MainWindow>().ToSelf().InSingletonScope();
            Kernel.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            Kernel.Load<ServiceModule>();
        }
    }
}
