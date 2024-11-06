using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Bookings.Commands;

public sealed class CreateBookingCommandHandler : ICommandHandler<CreateBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IEventBus _eventBus;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository, IEventBus eventBus, IFlightRepository flightRepository)
    {
        _bookingRepository = bookingRepository;
        _eventBus = eventBus;
        _flightRepository = flightRepository;
    }

    public async Task<Result> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        //Check User Exists
        if (!await _bookingRepository.CheckUserExist(request.UserId, cancellationToken))
        {
            return Result.Failure(DomainErrors.Booking.UserNotFound);
        }
        //Check Flight Exists
        if (!await _bookingRepository.CheckFlightExist(request.FlightId, cancellationToken))
        {
            return Result.Failure(DomainErrors.Booking.FlightNotFound);
        }

        var flight = await _flightRepository.GetByIdWithSeats(request.FlightId, cancellationToken);
        if ( flight!.Seats.Count(x => x.IsAvailable) < request.SeatIds.Count() )
        {
            return Result.Failure(DomainErrors.Seat.NotIsAvailable);
        }
        
        var booking = Booking.Create(BookingId.Create(),request.BookingDate,
            request.Status,request.UserId.Value,request.FlightId);

        var passengers = request.Passengers.Select(p =>
            Passenger.Create(PassengerId.Create(), p.Name, 
                p.PassportNumber, p.Nationality, p.DateOfBirth, booking.Id)).ToList();

        await _eventBus.PublishAsync(
            new BookingRegisterDomainEvent(booking,passengers,request.SeatIds),cancellationToken);
        
        return Result.Success();

    }
}