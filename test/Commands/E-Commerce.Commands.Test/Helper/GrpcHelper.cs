using E_Commerce.Commands.Test.Protos;
using Microsoft.AspNetCore.Mvc.Testing;

namespace E_Commerce.Commands.Test.Helper
{
    public class GrpcHelper(WebApplicationFactory<Program> factory)
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        public TResult Send<TResult>(Func<OrdersCommands.OrdersCommandsClient, TResult> send)
        {
            OrdersCommands.OrdersCommandsClient client = new OrdersCommands.OrdersCommandsClient(_factory.CreateGrpcChannel());

            return send(client);
        }
    }
}
