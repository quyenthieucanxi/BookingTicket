namespace Domain.ValueObjects;

public readonly record struct AirlineId(Guid Value)
{
    public static AirlineId Empty { get; } = new(Guid.Empty);
    public static AirlineId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}