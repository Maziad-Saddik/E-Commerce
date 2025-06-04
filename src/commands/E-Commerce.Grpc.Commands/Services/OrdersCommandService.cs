using E_Commerce.Grpc;
using Extensions;
using Grpc.Core;
using MediatR;

namespace Services
{
    public class OrdersCommandService(IMediator mediator) : OrdersCommands.OrdersCommandsBase
    {
        private readonly IMediator _mediator = mediator;

        public override async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
            => new CancelOrderResponse
            {
                Id = await _mediator.Send(request.ToCommand(), context.CancellationToken)
            };

        public async override Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
            => new PlaceOrderResponse
            {
                Id = await _mediator.Send(request.ToCommand(), context.CancellationToken)
            };
    }
}
