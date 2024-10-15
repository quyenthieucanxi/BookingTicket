using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public class Name : ValueObject
{
    public const int MaxLength = 40;
    public const int MinLength = 12;
    
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Name>(new Error(
                "Name.Empty",
                "Name is empty"
            ));
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<Name>(DomainErrors.Name.TooLong);
        }
        if (name.Length < MinLength)
        {
            return Result.Failure<Name>(DomainErrors.Name.TooShort);
        }
        return new Name(name);
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}