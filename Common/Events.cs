using Prism.Events;

namespace Common
{
    public class InstrumentAddedEvent : PubSubEvent<Instrument>
    {
    }

    public class InstrumentUpdatedEvent : PubSubEvent<Instrument>
    {
    }
}
