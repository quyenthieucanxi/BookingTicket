using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Flight : AggregateRoot<FlightId>
{
    private readonly List<Booking> _bookings = new();
    private readonly List<Seat> _seat = new();

    private Flight(FlightId id) : base(id)
    {
    }

    private Flight(FlightId id, string flightNumber, 
        DateTime departureTime, DateTime arrivalTime, 
        decimal price, int duration, AirportId originAirportId,
        AirportId destinationAirportId, AirlineId airlineId) : base(id)
    {
        FlightNumber = flightNumber;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        Price = price;
        Duration = duration;
        OriginAirportId = originAirportId;
        DestinationAirportId = destinationAirportId;
        AirlineId = airlineId;
    }

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

    public static Flight Create(FlightId id, string flightNumber,
        DateTime departureTime, DateTime arrivalTime, 
        decimal price, int duration, AirportId originAirportId,
        AirportId destinationAirportId, AirlineId airlineId)
    {
        return new(id, flightNumber, departureTime,
            arrivalTime, price, 
            duration, originAirportId, destinationAirportId,
            airlineId);
    }

    public void AddSeat(Seat seat)
    {
        _seat.Add(seat);
    }
    
}