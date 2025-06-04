using E_Commerce.Domain.Constants;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Domain.Events.Data
{
    public class OrderPlacedData
    {
        public required Customer Customer { get; init; }

        public required List<OrderItem> OrderItems { get; init; } = [];

        public OrderStatus OrderStatus { get; init; }
    }
}
