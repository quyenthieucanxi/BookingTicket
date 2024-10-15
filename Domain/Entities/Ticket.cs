using Domain.Enums;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Ticket : Entity<TicketId>
{
    private Ticket ()   
    {
        
    }
    public string SeatNumber { get; private set; }
    public TicketClass Class { get; private set; }
    public DateTime IssueDate { get; private set; }
    
    // Foreign Key
    public BookingId BookingId { get; private set; }
    public Booking Booking { get; private set; }
}