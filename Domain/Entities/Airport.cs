using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Airport : AggregateRoot<AirportId>
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Code { get; set; }
    
    // Navigation Properties
}