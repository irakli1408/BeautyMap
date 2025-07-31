using BeautyMap.Common.ErrorHandler.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BeautyMap.Application.Common.Behaviour
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                ValidationContext<TRequest> context = new(request);
                ValidationResult[] validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
                List<ValidationFailure> failures = validationResults.SelectMany(result => result.Errors).Where(failure => failure != null).ToList();

                if (failures.Any())
                {
                    throw new AppValidationException(
                        failures
                            .GroupBy(f => f.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(f => f.ErrorMessage).ToArray()
                            )
                    );
                }

            }
            return await next();
        }
    }
}
