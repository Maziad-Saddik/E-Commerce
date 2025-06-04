using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Domain.Commands
{
    public class PlaceOrderCommand : IRequest<string>
    {
        public required string OrderId { get; init; }

        public required string UserId { get; init; }

        public required Customer Customer { get; init; }
    }
}
