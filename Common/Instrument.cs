namespace Common
{
    public enum Source
    {
        Bloomberg,
        Tradeweb,
        Composite
    }

    public class Instrument : BaseModel
    {
        #region Private Members

        private string _cusip;
        private double _bid;
        private double _coupon;
        private Source _source;

        #endregion

        #region Public Properties

        public string Cusip
        {
            get
            {
                return _cusip;
            }
            set
            {
                if (_cusip != value)
                {
                    _cusip = value;
                    RaisePropertyChanged("Cusip");
                }
            }
        }

        public double Bid
        {
            get
            {
                return _bid;
            }
            set
            {
                if (_bid != value)
                {
                    _bid = value;
                    RaisePropertyChanged("Bid");
                }
            }
        }

        public double Coupon
        {
            get
            {
                return _coupon;
            }
            set
            {
                if (_coupon != value)
                {
                    _coupon = value;
                    RaisePropertyChanged("Coupon");
                }
            }
        }

        public Source Source
        {
            get
            {
                return _source;
            }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaisePropertyChanged("Source");
                }
            }
        }

        #endregion

        #region Public Methods

        public override void UpdateWith(BaseModel newModel)
        {
            var newInstrument = newModel as Instrument;

            this.Bid = newInstrument.Bid;
            this.Coupon = newInstrument.Coupon;
            this.Source = newInstrument.Source;
        }

        #endregion
    }
}
