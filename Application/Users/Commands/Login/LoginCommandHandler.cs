using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand,LoginResponse>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly ICacheService _cacheService;   
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<User> _signInManager;

    public LoginCommandHandler(IJwtProvider jwtProvider, 
        ICacheService cacheService, 
        IUserRepository userRepository, 
        SignInManager<User> signInManager)
    {
        _jwtProvider = jwtProvider;
        _cacheService = cacheService;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //Check User
        var user = await _userRepository.FindAsync(u => u.Email == request.Email, cancellationToken);
        if (user is null)
        {
            return Result.Failure<LoginResponse>(DomainErrors.User.EmailNotFound);
        }

        var checkLogin = await _signInManager.CheckPasswordSignInAsync(user, request.PassWord, false);
        if (!checkLogin.Succeeded)
        {
            return Result.Failure<LoginResponse>(DomainErrors.User.PasswordWrong);
        }
        //Generate Token
        var accessTokenKey = $"accessToken:{request.Email}";
        var refreshTokenKey = $"refreshToken:{request.Email}";

        string? accessToken = await _cacheService.GetAsync<string>(accessTokenKey,cancellationToken);

        if (string.IsNullOrEmpty(accessToken))
        {
            accessToken = _jwtProvider.Generate(user);
            await _cacheService.SetAsync(accessTokenKey,accessToken , cancellationToken,new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        }
        string? refreshToken = await _cacheService.GetAsync<string>(refreshTokenKey,cancellationToken);
        if (string.IsNullOrEmpty(refreshToken))
        {
            refreshToken = _jwtProvider.GenerateRefreshToken();
            await _cacheService.SetAsync(refreshTokenKey,refreshToken , cancellationToken,new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(5)
            });
        }

        
        return  new LoginResponse () { AccessToken = accessToken, RefeshToken = refreshToken };

    }
}