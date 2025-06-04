using E_Commerce.Domain.Constants;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Interfaces;

namespace E_Commerce.Domain.Aggregates
{
    public class Order : Aggregate<Order>, IAggregate
    {
        public OrderStatus _status { private get; set; }

        private Order() { }

        protected override void Mutate(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
