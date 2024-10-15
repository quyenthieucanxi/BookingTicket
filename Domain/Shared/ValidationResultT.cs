namespace Domain.Shared;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult

{
    internal ValidationResult(Error[] errors)
        : base(default, false, IValidationResult.ValidationError)
        => Errors = errors;
    public Error[] Errors { get; }
    
    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}