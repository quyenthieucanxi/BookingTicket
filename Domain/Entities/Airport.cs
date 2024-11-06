using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Airport : AggregateRoot<AirportId>
{
    private Airport(AirportId id) : base(id)
    {
        
    }
    public Airport(AirportId id, string name, string location, string code) : base(id)
    {
        Name = name;
        Location = location;
        Code = code;
    }

    public string Name { get; set; }
    public string Location { get; set; }
    public string Code { get; set; }
    
    // Navigation Properties

    public static Airport Create(AirportId id, string name, string location, string code)
    {
        return new(id, name, location, code);
    }
}