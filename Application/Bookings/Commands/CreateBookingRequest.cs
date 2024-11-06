using Application.Passengers.Commands;
using Domain.ValueObjects;

namespace Application.Bookings.Commands;

public  record CreateBookingRequest(DateTime BookingDate, string Status, 
    Guid UserId, Guid FlightId,ICollection<CreatePassengerRequest> Passengers, ICollection<Guid> SeatIds);