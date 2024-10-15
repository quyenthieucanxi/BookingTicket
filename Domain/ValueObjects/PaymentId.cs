namespace Domain.ValueObjects;

public readonly record struct PaymentId(Guid Value)
{
    public static PaymentId Empty { get; } = new(Guid.Empty);
    public static PaymentId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}