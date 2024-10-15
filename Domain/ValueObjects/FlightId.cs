namespace Domain.ValueObjects;

public readonly record struct FlightId(Guid Value)
{
    public static FlightId Empty { get; } = new(Guid.Empty);
    public static FlightId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}