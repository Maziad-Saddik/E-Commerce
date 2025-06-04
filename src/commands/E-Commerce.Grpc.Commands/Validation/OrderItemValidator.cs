using E_Commerce.Grpc;
using FluentValidation;

namespace Validation
{
    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.ProductRefenence)
                .NotEmpty()
                .Must(ValidationHelpers.IsValidGuid);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.Price)
               .GreaterThan(0);

            RuleFor(x => x.Currency)
              .NotEmpty()
              .Must(ValidationHelpers.ISValidCurrency);
        }
    }
}
