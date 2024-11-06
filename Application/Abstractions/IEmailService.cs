using Domain.Entities;

namespace Application.Abstractions;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(User user, CancellationToken cancellationToken = default);
    Task SendBookingEmailAsync(Booking booking, CancellationToken cancellationToken = default);

}