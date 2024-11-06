namespace Domain.ValueObjects;

public readonly record struct SeatId(Guid Value)
{
    public static SeatId Empty => new(Guid.Empty);
    public static SeatId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}