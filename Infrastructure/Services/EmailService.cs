using Application.Abstractions;
using Domain.Entities;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendWelcomeEmailAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SendBookingEmailAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}