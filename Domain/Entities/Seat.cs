using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Seat : Entity<SeatId>
{
    private Seat(SeatId id) : base(id)
    {
    }


    private Seat(SeatId id, string seatNumber, string @class, bool isAvailable, FlightId flightId) : base(id)
    {
        SeatNumber = seatNumber;
        Class = @class;
        IsAvailable = isAvailable;
        FlightId = flightId;
    }

    public string SeatNumber { get; set; }
    public string Class { get; set; }
    public bool IsAvailable { get; set; }
    
    // Foreign Key
    public FlightId FlightId { get; set; }
    public Flight Flight { get; set; }

    public static Seat Create(SeatId id, string seatNumber, 
        string @class, bool isAvailable, FlightId flightId)
    {
        return new(id,seatNumber,@class,isAvailable,flightId);
    }
}