using Application.Abstractions.Messaging;
using Application.Passengers.Commands;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Bookings.Commands;

public sealed record CreateBookingCommand(DateTime BookingDate, BookingStatus Status, 
    UserId UserId, FlightId FlightId, ICollection<CreatePassengerRequest> Passengers,
    ICollection<SeatId> SeatIds) : ICommand ;
    