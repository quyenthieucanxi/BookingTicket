using Domain.Abstractions;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using MassTransit;

namespace Application.Bookings.Events;

public sealed class BookingRegisterDomainEventConsumer : IConsumer<BookingRegisterDomainEvent>
{
    private readonly IFlightRepository _flightRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookingRegisterDomainEventConsumer(IFlightRepository flightRepository, 
        ISeatRepository seatRepository, IUnitOfWork unitOfWork, 
        IBookingRepository bookingRepository)
    {
        _flightRepository = flightRepository;
        _seatRepository = seatRepository;
        _unitOfWork = unitOfWork;
        _bookingRepository = bookingRepository;
    }

    public async Task Consume(ConsumeContext<BookingRegisterDomainEvent> context)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var fligt = await 
                _flightRepository.GetByIdWithSeats(context.Message.Booking.FlightId,context.CancellationToken);
            for (int i = 0; i < context.Message.Passengers.Count(); i++)
            {
                var seat = fligt!.Seats.FirstOrDefault(s => s.Id == context.Message.SeatIds.ElementAt(i));
                if (seat is  null)
                {
                    return;
                }
                seat.IsAvailable = false;
                var ticket = Ticket
                    .Create(TicketId.Create(),TicketClass.Economy,DateTime.UtcNow, context.Message.Booking.Id, seat!.Id);
                
                context.Message.Booking.AddTicket(ticket);
                context.Message.Booking.AddPassenger(context.Message.Passengers.ElementAt(i));
                _seatRepository.Update(seat);
            }
            _bookingRepository.Add(context.Message.Booking);
            await _unitOfWork.SaveChanges(context.CancellationToken);
            transaction.Commit();
            return;
            
        }
        catch (Exception e)
        {
            transaction.Rollback();
        }
    }
}