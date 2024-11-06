using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery,UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id.Value,cancellationToken);
    
        if (user is  null)
        {
            return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
        }
        var userResponse = new UserResponse(new UserId(user!.Id), user.Email);
        return userResponse;
    }
}