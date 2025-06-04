using E_Commerce.Domain.Commands;
using E_Commerce.Domain.Constants;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Exceptions;
using E_Commerce.Domain.Extensions;
using E_Commerce.Domain.Interfaces;

namespace E_Commerce.Domain.Aggregates
{
    public class Order : Aggregate<Order>, IAggregate
    {
        public OrderStatus _status { private get; set; }

        private Order() { }

        public static Order Place(PlaceOrderCommand command)
        {
            Order order = new Order();

            order.ApplyNewChange(command.ToOrderPlaced());

            return order;
        }

        protected override void Mutate(Event @event)
        {
            switch (@event)
            {
                case OrderPlaced e: Mutate(e); break;
                default:
                    throw new AppException(ExceptionStatusCode.FailedPrecondition, $"Unhandled event type: {@event.GetType().Name}"
                );
            }
        }

        protected void Mutate(OrderPlaced @event)
        {

        }
    }
}
