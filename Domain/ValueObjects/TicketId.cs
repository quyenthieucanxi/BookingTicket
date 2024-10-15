namespace Domain.ValueObjects;

public readonly record struct TicketId(Guid Value)
{
    public static TicketId Empty { get; } = new(Guid.Empty);
    public static TicketId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}