using E_Commerce.Grpc;
using Grpc.Core;

namespace Services
{
    public class OrdersCommandService(ILogger<OrdersCommandService> logger) : OrdersCommands.OrdersCommandsBase
    {
        private readonly ILogger<OrdersCommandService> _logger = logger;

        public override Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
        {
            return base.CancelOrder(request, context);
        }

        public override Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            return base.PlaceOrder(request, context);
        }
    }
}
