using Common;
using Ninject.Modules;

namespace MarketDataService
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IDataService>().To<BloombergDataService>().InSingletonScope().Named(Source.Bloomberg.ToString());
            Kernel.Bind<IDataService>().To<TradewebDataService>().InSingletonScope().Named(Source.Tradeweb.ToString());
            Kernel.Bind<IDataService>().To<CompositeDataService>().InSingletonScope().Named(Source.Composite.ToString());
        }
    }
}
