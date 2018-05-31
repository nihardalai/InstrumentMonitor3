using Common;

namespace MarketDataService
{
    public class CompositeDataService : BaseDataService
    {
        public CompositeDataService() : base(Source.Composite)
        {
            // Override InsertOrUpdateCusip here for custom composition.
        }
    }
}
