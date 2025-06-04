using E_Commerce.Domain.Commands;
using E_Commerce.Domain.Constants;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Exceptions;
using E_Commerce.Domain.Extensions;
using E_Commerce.Domain.Interfaces;
using Stateless;

namespace E_Commerce.Domain.Aggregates
{
    public class Order : Aggregate<Order>, IAggregate
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Order() => InitializeStateMachine();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public OrderStatus _status { private get; set; }

        private StateMachine<OrderStatus, OrderTrigger> _stateMachine;

        public static Order Place(PlaceOrderCommand command)
        {
            Order order = new Order();

            order.ApplyNewChange(command.ToOrderPlaced());

            return order;
        }

        public void Cancel(CancelOrderCommand command)
        {
            ApplyNewChange(command.ToOrderCanceled());
        }

        protected override void Mutate(Event @event)
        {
            switch (@event)
            {
                case OrderPlaced e: Mutate(e); break;
                case OrderCanceled e: Mutate(e); break;
                default:
                    throw new AppException(ExceptionStatusCode.FailedPrecondition, $"Unhandled event type: {@event.GetType().Name}"
                );
            }
        }

        protected void Mutate(OrderPlaced @event)
        {
            _status = OrderStatus.Pending;
        }

        protected void Mutate(OrderCanceled @event)
        {
            _status = OrderStatus.Cancelled;
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine<OrderStatus, OrderTrigger>(
                () => _status,
                s => _status = s
            );

            _stateMachine.Configure(OrderStatus.Pending)
                .Permit(OrderTrigger.Confirmed, OrderStatus.Confirmed)
                .Permit(OrderTrigger.Cancelled, OrderStatus.Cancelled);

            _stateMachine.Configure(OrderStatus.Confirmed)
                .Permit(OrderTrigger.Shipped, OrderStatus.Shipped)
                .Permit(OrderTrigger.Cancelled, OrderStatus.Cancelled);

            _stateMachine.Configure(OrderStatus.Shipped)
                .Permit(OrderTrigger.Delivered, OrderStatus.Delivered);

            _stateMachine.Configure(OrderStatus.Delivered)
                .Ignore(OrderTrigger.Confirmed)
                .Ignore(OrderTrigger.Shipped)
                .Ignore(OrderTrigger.Delivered)
                .Ignore(OrderTrigger.Cancelled);

            _stateMachine.Configure(OrderStatus.Cancelled)
                .Ignore(OrderTrigger.Confirmed)
                .Ignore(OrderTrigger.Shipped)
                .Ignore(OrderTrigger.Delivered)
                .Ignore(OrderTrigger.Cancelled);
        }
    }
}
