using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(UserId Id) : IQuery<UserResponse>;