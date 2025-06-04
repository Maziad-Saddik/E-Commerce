using E_Commerce.Domain.Events;

namespace E_Commerce.Commands.Test.Helper
{
    public class EventsDataFakerHelpers
    {
        public Event this[int index]
        {
            get
            {
                return _events.ToArray()[index];
            }
        }

        private readonly List<Event> _events = new();

        public IReadOnlyList<Event> GetEvents() => _events;

        public TEvent First<TEvent>() where TEvent : Event => (TEvent)_events.First(e => e is TEvent);

        public TEvent Last<TEvent>() where TEvent : Event => (TEvent)_events.Last(e => e is TEvent);
    }
}
