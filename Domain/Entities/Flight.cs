using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Flight : AggregateRoot<FlightId>
{
    private readonly List<Booking> _bookings = new();
    private readonly List<Seat> _seat = new();
    public string FlightNumber { get; private set; } = default!;
    public DateTime DepartureTime { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    public decimal Price { get; private set; }
    public int Duration { get; private set;}
    
    //Foreign Keys
    public AirportId OriginAirportId { get; private set; }
    public AirportId DestinationAirportId { get; private set; }
    public AirlineId AirlineId { get; private set; }
    
    // Navigation Properties
    public IReadOnlyCollection<Booking> Bookings => _bookings;
    public IReadOnlyCollection<Seat> Seats => _seat;
    public Airport Origin { get; private set; }
    public Airport Destination { get; private set; } 
    public Airline Airline { get; private set; }
    
}