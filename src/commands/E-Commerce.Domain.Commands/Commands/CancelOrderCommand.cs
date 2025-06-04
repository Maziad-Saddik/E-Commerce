using E_Commerce.Domain.Interfaces;
using MediatR;

namespace E_Commerce.Domain.Commands
{
    public class CancelOrderCommand : IRequest<string>, ICommand
    {
        public required string OrderId { get; init; }

        public required string UserId { get; init; }
    }
}
