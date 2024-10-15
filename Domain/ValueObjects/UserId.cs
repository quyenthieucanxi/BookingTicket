namespace Domain.ValueObjects;

public readonly record struct UserId(Guid Value)
{
    public static UserId Empty { get; } = new(Guid.Empty);
    public static UserId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}