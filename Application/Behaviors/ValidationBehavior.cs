
using Application.Abstractions.Messaging;
using Domain.Shared;
using FluentValidation;
using MediatR;
using ValidationException = Application.Exceptions.ValidationException;


namespace Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                where TRequest : IRequest<TResponse>
                where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
    
    public async Task<TResponse> Handle(
        TRequest request, 
        CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any())
        {
            return await next();
        }
        var context = new ValidationContext<TRequest>(request);

        var errorsDictionary = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .Select(failure => new Error(
                failure.PropertyName,
                    failure.ErrorMessage
            ))
            .Distinct()
            .ToArray();
            // .GroupBy(
            //     x => x.PropertyName,
            //     x => x.ErrorMessage,
            //     (propertyName, errorMessages) => new
            //     {
            //         Key = propertyName,
            //         Values = errorMessages.Distinct().ToArray()
            //     })
            // .ToDictionary(x => x.Key, x => x.Values);

        if (errorsDictionary.Any())
        {
            //throw new ValidationException(errorsDictionary);
            return CreateValidationResult<TResponse>(errorsDictionary); 
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult )!;
        }

        object validationResult =  typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;
        return (TResult)validationResult ;
    }
}