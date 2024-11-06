using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.DomainEvents;

namespace Application.Users.Events;

public class UserRegisterDomainEventHandler : IDomainEventHandler<UserRegisterDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public UserRegisterDomainEventHandler(IEmailService emailService, IUserRepository userRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task Handle(UserRegisterDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.Id.Value, cancellationToken);

        if (user is null)
        {
            return;
        }
        await _emailService.SendWelcomeEmailAsync(user, cancellationToken);
    }
}