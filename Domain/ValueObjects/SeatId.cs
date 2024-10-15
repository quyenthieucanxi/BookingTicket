namespace Domain.ValueObjects;

public readonly record struct SeatId(Guid Value)
{
    public static BookingId Empty => new(Guid.Empty);
    public static BookingId Create => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}