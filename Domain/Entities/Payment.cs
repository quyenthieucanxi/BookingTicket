
using Domain.Enums;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Payment : Entity<PaymentId>
{
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentMethod  PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    
    //Foreign Key
    public BookingId BookingId { get; private set; }
    public Booking Booking { get; private set; }
}
