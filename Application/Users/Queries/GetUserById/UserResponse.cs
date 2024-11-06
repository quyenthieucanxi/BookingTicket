using Domain.ValueObjects;

namespace Application.Users.Queries.GetUserById;

public sealed record UserResponse(UserId Id,string Email);
