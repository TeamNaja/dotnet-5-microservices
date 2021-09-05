namespace Ordering.Application.Behaviours
{
    using FluentValidation;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ValidationException = Exceptions.ValidationException;

    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (validators.Any())
            {
                var validationContext = new ValidationContext<TRequest>(request);
                var validationResult = await Task.WhenAll(validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));
                var failures = validationResult.SelectMany(x => x.Errors).Where(x => x is not null).ToList();

                if (failures.Count > 0)
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
