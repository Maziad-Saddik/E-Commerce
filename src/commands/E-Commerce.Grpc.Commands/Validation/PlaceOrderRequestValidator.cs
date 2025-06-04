using E_Commerce.Grpc;
using FluentValidation;

namespace Validation
{
    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
    {
        public PlaceOrderRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .Must(ValidationHelpers.IsValidGuid);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(ValidationHelpers.IsValidGuid);

            RuleFor(x => x.Customer.Id)
                .NotEmpty()
                .Must(ValidationHelpers.IsValidGuid);

            RuleFor(x => x.Customer.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Customer.Email)
                .NotEmpty()
                .Must(ValidationHelpers.ISValidEmail);

            RuleFor(x => x.OrderItems)
                .NotEmpty()
                .Must(x => x.Count > 0)
                .WithMessage("OrderItems must have at least one item.");

            RuleForEach(x => x.OrderItems)
                .SetValidator(new OrderItemValidator());
        }
    }
}
