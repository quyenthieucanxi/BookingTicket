namespace Domain.ValueObjects;

public readonly record struct AirlineId(Guid Value)
{
    public static AirlineId Empty { get; } = new(Guid.Empty);
    public static AirlineId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
    
    public static bool TryParse(string input, out AirlineId airlineId)
    {
        airlineId = AirlineId.Empty;
        if (Guid.TryParse(input, out var guid))
        {
            airlineId = new AirlineId(guid);
            return true;
        }
        return false;
    }

}