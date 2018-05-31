using Common;
using MarketDataService;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace InstrumentMonitor
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _bloombergSelectedCusip;
        private string _tradewebSelectedCusip;
        private string _compositeSelectedCusip;

        private ObservableCollection<Instrument> _bloombergInstruments;
        private ObservableCollection<Instrument> _tradewebInstruments;
        private ObservableCollection<Instrument> _compositeInstruments;

        private static object _bbgSyncObject = new object();
        private static object _twSyncObject = new object();
        private static object _compositeSyncObject = new object();

        private readonly IDataService _bbgDataService;
        private readonly IDataService _twDataService;
        private readonly IDataService _compositeDataService;
        private readonly IEventAggregator _eventAggregator;

        private ICommand _startCommand;
        private ICommand _stopCommand;
        private ICommand _subscribeCommand;
        private ICommand _unsubscribeCommand;

        public ObservableCollection<Instrument> BloombergInstruments
        {
            get
            {
                return _bloombergInstruments;
            }
            set
            {
                _bloombergInstruments = value;
                RaisePropertyChanged("BloombergInstruments");
            }
        }

        public ObservableCollection<Instrument> TradewebInstruments
        {
            get
            {
                return _tradewebInstruments;
            }
            set
            {
                _tradewebInstruments = value;
                RaisePropertyChanged("TradewebInstruments");
            }
        }

        public ObservableCollection<Instrument> CompositeInstruments
        {
            get
            {
                return _compositeInstruments;
            }
            set
            {
                _compositeInstruments = value;
                RaisePropertyChanged("CompositeInstruments");
            }
        }

        public ICommand StartCommand
        {
            get
            {
                return _startCommand;
            }
            set
            {
                _startCommand = value;
                RaisePropertyChanged("StartCommand");
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand;
            }
            set
            {
                _stopCommand = value;
                RaisePropertyChanged("StopCommand");
            }
        }

        public ICommand SubscribeCommand
        {
            get
            {
                return _subscribeCommand;
            }
            set
            {
                _subscribeCommand = value;
                RaisePropertyChanged("SubscribeCommand");
            }
        }

        public ICommand UnsubscribeCommand
        {
            get
            {
                return _unsubscribeCommand;
            }
            set
            {
                _unsubscribeCommand = value;
                RaisePropertyChanged("UnsubscribeCommand");
            }
        }

        public string BloombergSelectedCusip
        {
            get
            {
                return _bloombergSelectedCusip;
            }
            set
            {
                _bloombergSelectedCusip = value;
                RaisePropertyChanged("BloombergSelectedCusip");
            }
        }

        public string TradewebSelectedCusip
        {
            get
            {
                return _tradewebSelectedCusip;
            }
            set
            {
                _tradewebSelectedCusip = value;
                RaisePropertyChanged("TradewebSelectedCusip");
            }
        }

        public string CompositeSelectedCusip
        {
            get
            {
                return _compositeSelectedCusip;
            }
            set
            {
                _compositeSelectedCusip = value;
                RaisePropertyChanged("CompositeSelectedCusip");
            }
        }

        public MainWindowViewModel()
        {
            BloombergInstruments = new ObservableCollection<Instrument>();
            TradewebInstruments = new ObservableCollection<Instrument>();
            CompositeInstruments = new ObservableCollection<Instrument>();

            _startCommand = new RelayCommand(x => OnStart(x));
            _stopCommand = new RelayCommand(x => OnStop(x));
            _subscribeCommand = new RelayCommand(x => OnSubscribe(x));
            _unsubscribeCommand = new RelayCommand(x => OnUnsubscribe(x));

            var kernel = ServiceLocator.Current.GetInstance<IKernel>();

            _bbgDataService = kernel.Get<IDataService>(Source.Bloomberg.ToString());
            _twDataService = kernel.Get<IDataService>(Source.Tradeweb.ToString());
            _compositeDataService = kernel.Get<IDataService>(Source.Composite.ToString());

            _eventAggregator = kernel.Get<IEventAggregator>();
            _eventAggregator.GetEvent<InstrumentAddedEvent>().Subscribe(x => OnInstrumentAddedOrUpdated(x), ThreadOption.UIThread);
            _eventAggregator.GetEvent<InstrumentUpdatedEvent>().Subscribe(x => OnInstrumentAddedOrUpdated(x), ThreadOption.UIThread);
        }

        private void OnUnsubscribe(object x)
        {
            if (x == null)
                return;

            var source = x.ToString();
            if (source == Source.Bloomberg.ToString())
                _bbgDataService.Unsubscribe(BloombergSelectedCusip);
            else if (source == Source.Tradeweb.ToString())
                _twDataService.Unsubscribe(TradewebSelectedCusip);
            else if (source == Source.Composite.ToString())
                _compositeDataService.Unsubscribe(CompositeSelectedCusip);
        }

        private void OnSubscribe(object x)
        {
            if (x == null)
                return;

            var source = x.ToString();
            if (source == Source.Bloomberg.ToString())
                _bbgDataService.Subscribe(BloombergSelectedCusip);
            else if (source == Source.Tradeweb.ToString())
                _twDataService.Subscribe(TradewebSelectedCusip);
            else if (source == Source.Composite.ToString())
                _compositeDataService.Subscribe(CompositeSelectedCusip);
        }

        private void OnStop(object x)
        {
            if (x == null)
                return;

            var source = x.ToString();
            if (source == Source.Bloomberg.ToString())
                _bbgDataService.Stop();
            else if (source == Source.Tradeweb.ToString())
                _twDataService.Stop();
            else if (source == Source.Composite.ToString())
                _compositeDataService.Stop();
        }

        private void OnStart(object x)
        {
            if (x == null)
                return;

            var source = x.ToString();
            if (source == Source.Bloomberg.ToString())
                _bbgDataService.Start();
            else if (source == Source.Tradeweb.ToString())
                _twDataService.Start();
            else if (source == Source.Composite.ToString())
                _compositeDataService.Start();
        }

        private void OnInstrumentAddedOrUpdated(Instrument x)
        {
            if (x.Source == Source.Bloomberg)
            {
                lock (_bbgSyncObject)
                {
                    var ins = _bloombergInstruments.FirstOrDefault(m => m.Cusip == x.Cusip);
                    if (ins == null)
                        _bloombergInstruments.Add(x);
                    else
                        ins.UpdateWith(x);
                }
            }
            else if (x.Source == Source.Tradeweb)
            {
                lock (_twSyncObject)
                {
                    var ins = _tradewebInstruments.FirstOrDefault(m => m.Cusip == x.Cusip);
                    if (ins == null)
                        _tradewebInstruments.Add(x);
                    else
                        ins.UpdateWith(x);
                }
            }
            else if (x.Source == Source.Composite)
            {
                lock (_compositeSyncObject)
                {
                    var ins = _compositeInstruments.FirstOrDefault(m => m.Cusip == x.Cusip);
                    if (ins == null)
                        _compositeInstruments.Add(x);
                    else
                        ins.UpdateWith(x);
                }
            }
        }
    }
}
