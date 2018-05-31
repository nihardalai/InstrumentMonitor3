using Common;

namespace MarketDataService
{
    public class BloombergDataService : BaseDataService
    {
        public BloombergDataService() : base(Source.Bloomberg)
        {
        }
    }
}
