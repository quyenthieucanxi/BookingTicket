namespace Domain.ValueObjects;

public readonly record struct BookingId(Guid Value)
{
    public static BookingId Empty { get; } = new(Guid.Empty);
    public static BookingId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}