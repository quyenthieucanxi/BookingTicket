using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.DomainEvents;

namespace Application.Bookings.Events;

public class BookingRegisterDomainEventHandler : IDomainEventHandler<BookingRegisterDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IBookingRepository _bookingRepository;

    public BookingRegisterDomainEventHandler(IEmailService emailService, IBookingRepository bookingRepository)
    {
        _emailService = emailService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingRegisterDomainEvent notification, CancellationToken cancellationToken)
    {
    
        await _emailService.SendBookingEmailAsync(notification.Booking,cancellationToken);
    }
}