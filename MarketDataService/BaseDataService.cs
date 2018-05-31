using Common;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace MarketDataService
{
    public abstract class BaseDataService : IDataService
    {
        Random cusipRandomizer = new Random();
        Random priceRandomizer = new Random();
        Random couponRandomizer = new Random();

        Timer _publishTimer = new Timer(20);

        ConcurrentDictionary<string, Instrument> _instruments;
        ConcurrentDictionary<string, Instrument> _subscriptions;

        readonly IEventAggregator _eventAggregator;

        private Source _source;

        public BaseDataService(Source source)
        {
            _source = source;

            var kernel = ServiceLocator.Current.GetInstance<IKernel>();
            _eventAggregator = kernel.Get<IEventAggregator>();

            _publishTimer.Elapsed += Timer_Tick;

            _instruments = new ConcurrentDictionary<string, Instrument>();
            _subscriptions = new ConcurrentDictionary<string, Instrument>();
        }

        public virtual void Start()
        {
            _publishTimer.Start();
        }

        public virtual void Stop()
        {
            _publishTimer.Stop();
        }

        public virtual void Subscribe(string cusip)
        {
            _subscriptions.TryAdd(cusip, null);
        }

        public virtual void Unsubscribe(string cusip)
        {
            Instrument removedVal = null;
            _subscriptions.TryRemove(cusip, out removedVal);
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            Parallel.For(0, 99, (x) => InsertOrUpdateCusip(GetRandomInstrument()));
        }

        protected virtual void InsertOrUpdateCusip(Instrument ins)
        {
            if (_instruments.ContainsKey(ins.Cusip))
            {
                Console.WriteLine("Updating " + ins.Cusip);
                _instruments.TryUpdate(ins.Cusip, ins, ins);
                if (_subscriptions.ContainsKey(ins.Cusip))
                    _eventAggregator.GetEvent<InstrumentUpdatedEvent>().Publish(ins);
            }
            else
            {
                Console.WriteLine("Adding " + ins.Cusip);
                _instruments.TryAdd(ins.Cusip, ins);
                _subscriptions.TryAdd(ins.Cusip, null);
                _eventAggregator.GetEvent<InstrumentAddedEvent>().Publish(ins);
            }
        }

        string GetRandomCusip()
        {
            var rnd = cusipRandomizer.Next(1, 100);
            bool flag = rnd % 2 == 0;
            if (flag)
                rnd = 101 - rnd;

            return "Cusip" + rnd;
        }

        double GetRandomPrice()
        {
            return Math.Round(priceRandomizer.NextDouble() * 100, 4);
        }

        double GetRandomCoupon()
        {
            return Math.Round(couponRandomizer.NextDouble(), 2);
        }

        Instrument GetRandomInstrument()
        {
            return CreateInstrument(GetRandomCusip(), GetRandomPrice(), GetRandomCoupon());
        }

        Instrument CreateInstrument(string cusip, double price, double coupon)
        {
            return new Instrument { Cusip = cusip, Bid = price, Coupon = coupon, Source = _source };
        }
    }
}
