using MediatR;

namespace E_Commerce.Domain.Commands
{
    public class CancelOrderCommand : IRequest<string>
    {
        public required string OrderId { get; init; }

        public required string UserId { get; init; }
    }
}
