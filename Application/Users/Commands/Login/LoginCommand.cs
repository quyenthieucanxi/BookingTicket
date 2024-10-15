using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Users.Commands.Login;

public sealed record LoginCommand(string Email, string PassWord): ICommand<LoginResponse>;