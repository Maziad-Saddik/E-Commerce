using Bogus;
using E_Commerce.Domain.Events;

namespace E_Commerce.Commands.Test.Faker.Events
{
    public abstract class EventFaker<TEvent, TData> : Faker<TEvent>
        where TEvent : Event<TData>
    {
        protected EventFaker()
        {
            RuleFor(e => e.AggregateId, Guid.NewGuid().ToString());

            RuleFor(x => x.Sequence, f => 1);

            RuleFor(x => x.DateTime, f => f.Date.PastOffset(1).UtcDateTime);

            RuleFor(x => x.Version, f => 1);

            RuleFor(x => x.UserId, f => f.Person.UserName);
        }
    }
}
