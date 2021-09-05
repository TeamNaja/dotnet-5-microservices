namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    using FluentValidation;

    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty().WithMessage("{UserName} is required")
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} must greater than zero.");
        }
    }
}
