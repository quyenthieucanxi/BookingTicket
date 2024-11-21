using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery,UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cache;

    public GetUserByIdQueryHandler(IUserRepository userRepository, ICacheService cache)
    {
        _userRepository = userRepository;
        _cache = cache;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        string userIdKey = $"{request.Id.Value}";

        var user = await _cache.GetAsync<User?>(userIdKey, cancellationToken);
        
        if (user is  null)
        {
            user = await _userRepository.GetByIdAsync(request.Id.Value, cancellationToken);
            if (user is null)
            {
                return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
            }
            await _cache.SetAsync(userIdKey,user,cancellationToken);
        }

        return new UserResponse(new UserId(user!.Id), user.Email);
    }
}