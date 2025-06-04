using E_Commerce.Domain.Entities;

namespace E_Commerce.Domain.Events.Data
{
    public class OrderPlacedData
    {
        public required Customer Customer { get; init; }
    }
}
