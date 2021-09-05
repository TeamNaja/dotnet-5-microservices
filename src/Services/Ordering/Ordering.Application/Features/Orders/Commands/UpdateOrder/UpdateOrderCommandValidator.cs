namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    using FluentValidation;

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
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
