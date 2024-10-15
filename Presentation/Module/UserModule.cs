using Application.Users.Commands.CreateUser;
using Application.Users.Commands.Login;
using Carter;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class UserModule : ModuleBase, ICarterModule
{
    public const string Tags = "Users";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login",Login)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        
        app.MapPost("users/register",Register)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
    public async Task<IResult> Login(
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

    public async Task<IResult> Register(
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
    
    
}