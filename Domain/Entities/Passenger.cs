using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Passenger : Entity<PassengerId>
{
    
    public string Name { get; set; }
    public string PassportNumber { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    // Foreign Key
    public BookingId BookingId { get; set; }
    public Booking Booking { get; set; }
}