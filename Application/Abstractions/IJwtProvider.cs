using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;


namespace Application.Abstractions;


public interface IJwtProvider
{
    string Generate(User user);
    string GenerateRefreshToken();

    Result<UserId> Decode() ;
}