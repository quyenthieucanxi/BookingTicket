using System.Text.Json.Serialization;
using Domain.DomainEvents;
using Domain.Enums;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Booking : AggregateRoot<BookingId>
{
    private readonly List<Ticket> _tickets = new();
    private readonly List<Passenger> _passengers = new();
    private Booking(BookingId id) : base(id)
    {
    }
    private Booking(BookingId id, DateTime bookingDate, 
        BookingStatus status, Guid userId, FlightId flightId) : base(id)
    {
        BookingDate = bookingDate;
        Status = status;
        UserId = userId;
        FlightId = flightId;
    }


    public Booking()
    {
    }

    public DateTime BookingDate { get; private set; }
    public BookingStatus Status { get; private set; }
   
    //Foreign Keys
    public Guid UserId { get; private set; }
    public FlightId FlightId { get; private set; }  
    public User User { get; private set; }
    public Flight Flight { get; private set; }
    
    //Navigation Properties
    public IReadOnlyCollection<Ticket> Tickets => _tickets;
    public IReadOnlyCollection<Passenger> Passengers => _passengers;
    public Payment Payment { get; private set; }

    public static Booking Create(BookingId id, DateTime bookingDate, 
        BookingStatus status, Guid userId, FlightId flightId)
    {
        var booking = new Booking(id, bookingDate, status, userId, flightId);
        return booking;
    }

    public void AddTicket(Ticket ticket)
    {
        _tickets.Add(ticket);
    }

    public void AddPassenger(Passenger passenger)
    {
        _passengers.Add(passenger);
    }
    
    
}