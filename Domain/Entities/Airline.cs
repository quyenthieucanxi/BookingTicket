using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed  class Airline : AggregateRoot<AirlineId>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string IATACode { get; set; }

    // Navigation Properties
    public IReadOnlyCollection<Flight> Flights { get; set; }
}