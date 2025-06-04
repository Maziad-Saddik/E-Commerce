using Bogus;
using E_Commerce.Commands.Test.Protos;

namespace Anis.TransactionsDateManagement.Commands.Test.Fakers.RequestFaker
{
    public class PlaceOrderRequestFaker : Faker<PlaceOrderRequest>
    {
        public PlaceOrderRequestFaker()
        {
            RuleFor(x => x.UserId, f => f.Random.Guid().ToString());
            RuleFor(x => x.OrderId, f => f.Random.Guid().ToString());
            RuleFor(x => x.Customer, f => new Customer
            {
                Id = f.Random.Guid().ToString(),
                Email = f.Person.Email,
                Name = f.Person.FirstName,
            });
            RuleFor(x => x.OrderItems, f => []);
        }
    }
}
