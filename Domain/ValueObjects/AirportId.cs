namespace Domain.ValueObjects;

public readonly record struct AirportId(Guid Value)
{
    public static AirportId Empty => new(Guid.Empty);
    public static AirportId Create => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}