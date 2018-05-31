namespace MarketDataService
{
    public interface IDataService
    {
        void Start();

        void Stop();

        void Subscribe(string cusip);

        void Unsubscribe(string cusip);
    }
}
