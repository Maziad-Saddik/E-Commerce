using Bogus;
using E_Commerce.Commands.Test.Protos;

namespace E_Commerce.Commands.Test.Faker.Requests
{
    public class CancelOrderRequestFaker : Faker<CancelOrderRequest>
    {
        public CancelOrderRequestFaker()
        {
            RuleFor(x => x.UserId, f => f.Random.Guid().ToString());
            RuleFor(x => x.OrderId, f => f.Random.Guid().ToString());
        }
    }

}
