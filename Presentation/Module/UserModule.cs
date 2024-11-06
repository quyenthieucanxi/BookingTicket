using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Abstractions;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.Login;
using Application.Users.Queries.GetUserById;
using Carter;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class UserModule : ModuleBase, ICarterModule
{
    public const string Tags = "Users";
    private readonly IJwtProvider _jwtProvider;

    public UserModule(IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login",Login)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        
        app.MapPost("/users/register",Register)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        app.MapGet("/users/{id}", GetUserById)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        
        app.MapGet("/users/myInfo", GetMyInfo)
            .RequireAuthorization()
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        

    }

    private async Task<IResult> GetMyInfo(ISender sender,CancellationToken cancellationToken)
    {
        Result<UserId> userIdResult = _jwtProvider.Decode();
        if (userIdResult.IsFailure)
        {
            return HandleFailure(userIdResult);
        }
        var query = new GetUserByIdQuery(userIdResult.Value);
        Result<UserResponse> result = await sender.Send(query,cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Results.Ok(result.Value);
    }

    private async Task<IResult> Login(
        LoginRequest request ,ISender sender, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email,request.Password);
        Result<LoginResponse> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Results.Ok(result.Value);
    }

    private async Task<IResult> Register(
        CreateUserRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.Email,request.UserName,request.Password,request.Name);
        Result<UserId> result = await sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result.Value);
    }
    private async Task<IResult> GetUserById(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(new UserId(id));
        Result<UserResponse> result = await sender.Send(query,cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Results.Ok(result.Value);
    }
    
}