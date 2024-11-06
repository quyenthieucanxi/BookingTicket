using Domain.Enums;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Ticket : Entity<TicketId>
{

    private Ticket(TicketId id) : base(id)
    {
    }

    private Ticket(TicketId id, TicketClass @class, DateTime issueDate
        , BookingId bookingId, SeatId seatId) : base(id)
    {
        Class = @class;
        IssueDate = issueDate;
        BookingId = bookingId;
        SeatId = seatId;
    }

    public TicketClass Class { get; private set; }
    public DateTime IssueDate { get; private set; }
    
    // Foreign Key
    public BookingId BookingId { get; private set; }
    public Booking Booking { get; private set; }
    
    public SeatId SeatId { get; private set; } 
    
    public Seat Seat { get; private set; }

    public static Ticket Create(TicketId id, TicketClass @class, DateTime issueDate
        , BookingId bookingId, SeatId seatId)
        => new(id, @class, issueDate, bookingId, seatId);   
}