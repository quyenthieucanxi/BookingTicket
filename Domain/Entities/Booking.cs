using Domain.Enums;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Booking : AggregateRoot<BookingId>
{
    private readonly List<Ticket> _ticket = new();
    private readonly List<Passenger> _passengers = new();

    
    public DateTime BookingDate { get; private set; }
    public BookingStatus Status { get; private set; }
   
    //Foreign Keys
    public Guid UserId { get; private set; }
    public FlightId FlightId { get; private set; }  
    public User User { get; private set; }
    public Flight Flight { get; private set; }
    
    //Navigation Properties
    public IReadOnlyCollection<Ticket> Tickets => _ticket;
    public IReadOnlyCollection<Passenger> Passengers => _passengers;
    public Payment Payment { get; private set; }
    
}