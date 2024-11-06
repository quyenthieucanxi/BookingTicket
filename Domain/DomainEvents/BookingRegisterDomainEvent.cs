using Domain.Entities;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed record BookingRegisterDomainEvent(Booking Booking, 
    ICollection<Passenger> Passengers,
    ICollection<SeatId> SeatIds) : IDomainEvent;