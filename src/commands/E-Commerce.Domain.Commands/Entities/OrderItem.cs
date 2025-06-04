using E_Commerce.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace E_Commerce.Domain.Entities
{
    public class OrderItem
    {
        [JsonConstructor]
        private OrderItem(
            string productRef,
            int quantity,
            Money price
        )
        {
            ProductRef = productRef;
            Quantity = quantity;
            Price = price;
        }

        public string ProductRef { get; private set; }
        public int Quantity { get; private set; }
        public Money Price { get; private set; }

        public static OrderItem Add(string productRef, int quantity, Money price)
            => new(
                productRef: productRef,
                quantity: quantity,
                price: price
            );
    }
}
