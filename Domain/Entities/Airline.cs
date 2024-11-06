using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed  class Airline : AggregateRoot<AirlineId>
{
    private Airline(AirlineId id) : base(id)
    {
        
    }

    private Airline(AirlineId id, string name, string country, string iataCode) : base(id)
    {
        Name = name;
        Country = country;
        IATACode = iataCode;
    }

    public string Name { get; set; }
    public string Country { get; set; }
    public string IATACode { get; set; }

    // Navigation Properties
    public IReadOnlyCollection<Flight> Flights { get; set; }

    public static Airline Create(AirlineId id,string name,string country,string iataCode)
    {
        return new(id, name, country, iataCode);
    }
}