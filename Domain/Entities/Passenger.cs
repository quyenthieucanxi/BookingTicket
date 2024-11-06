using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Passenger : Entity<PassengerId>
{
    private Passenger(PassengerId id) : base(id)
    {
    }


    
    private Passenger(PassengerId id, string name, string passportNumber, 
        string nationality, DateTime dateOfBirth, BookingId bookingId) : base(id)
    {
        Name = name;
        PassportNumber = passportNumber;
        Nationality = nationality;
        DateOfBirth = dateOfBirth;
        BookingId = bookingId;
    }

    public Passenger()
    {
    }

    public string Name { get; set; }
    public string PassportNumber { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    // Foreign Key
    public BookingId BookingId { get; set; }
    public Booking Booking { get; set; }


    public static Passenger Create(PassengerId id, string name, string passportNumber,
        string nationality, DateTime dateOfBirth, BookingId bookingId)
    {
        return new (id,name,passportNumber,nationality,dateOfBirth,bookingId);
    }
    
}

