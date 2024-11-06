using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed record UserRegisterDomainEvent(UserId Id) : IDomainEvent;
