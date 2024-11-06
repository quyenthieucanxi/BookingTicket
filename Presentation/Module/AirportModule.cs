using Application.Airports.Commands;
using Application.Airports.Queries;
using Carter;
using Domain.Abstractions;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class AirportModule : ModuleBase,ICarterModule
{
    public const string Tags = "Airports";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/airports/create",CreateAirport)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        app.MapGet("/airports/{id}",GetAirportById)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAirportById(Guid id, ISender sender,CancellationToken cancellationToken)
    {
        var query = new GetAirportByIdQuery(new AirportId(id));

        Result<AirportResponse> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result.Value);
    }

    private async Task<IResult> CreateAirport(CreateAirportCommand request, 
        ISender sender,CancellationToken cancellationToken)
    {
        var command = new CreateAirportCommand(request.Name,request.Location,request.Code);
        Result result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Results.Ok(result);
    }
}