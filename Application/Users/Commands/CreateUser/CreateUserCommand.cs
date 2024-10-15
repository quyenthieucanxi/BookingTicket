using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Email,string UserName,string Password,string Name) : ICommand<UserId>;