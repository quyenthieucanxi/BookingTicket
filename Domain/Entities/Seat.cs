using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Seat : Entity<SeatId>
{
    public string SeatNumber { get; set; }
    public string Class { get; set; }
    public bool IsAvailable { get; set; }
    
    // Foreign Key
    public FlightId FlightId { get; set; }
    public Flight Flight { get; set; }
}