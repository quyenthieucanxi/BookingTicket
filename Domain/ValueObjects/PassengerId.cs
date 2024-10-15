namespace Domain.ValueObjects;

public readonly record struct PassengerId(Guid Value)
{
    public static PassengerId Empty { get; } = new(Guid.Empty);
    public static PassengerId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}