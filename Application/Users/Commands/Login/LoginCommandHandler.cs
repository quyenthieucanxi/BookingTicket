using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Users.Commands.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand,LoginResponse>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly ICacheService _cacheService;

    public LoginCommandHandler(IJwtProvider jwtProvider, ICacheService cacheService)
    {
        _jwtProvider = jwtProvider;
        _cacheService = cacheService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //Check User
        
        //Generate Token
        var accessTokenKey = $"accessToken:{request.Email}";
        var refreshTokenKey = $"refreshToken:{request.Email}";
        
        string accessToken = _jwtProvider.Generate(null);
        string refreshToken = _jwtProvider.GenerateRefreshToken();

        await _cacheService.SetAsync(accessTokenKey,accessToken , cancellationToken);
        await _cacheService.SetAsync(refreshTokenKey, refreshToken, cancellationToken);
        
        return  new LoginResponse () { AccessToken = accessToken, RefeshToken = refreshToken };

    }
}