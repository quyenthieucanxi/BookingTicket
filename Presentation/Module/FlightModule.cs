using Application.Flights.Commands;
using Application.Flights.Queries;
using Carter;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class FlightModule : ModuleBase, ICarterModule
{
    public const string Tags = "Filghts";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/flights/create",CreateFlight)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        app.MapGet("/flights", GetFlightsQuery)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetFlightsQuery(string? searchTerm, string? sortColumn, 
        string? sortOrder, int page, int pageSize, ISender sender)
    {
        var query = new GetFlightsQuery(searchTerm,sortColumn,sortOrder,page,pageSize);

        Result<PageList<FlightResponse>> result = await sender.Send(query);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result.Value);
    }

    private async Task<IResult> CreateFlight(CreateFlightRequest request,ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateFlightCommand(request.FlightNumber, request.DepartureTime, request.ArrivalTime,
            request.Price, request.Duration,new AirportId(request.OriginAirportId) , new AirportId( request.DestinationAirportId),
            new AirlineId(request.AirlineId));

        Result result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
             return HandleFailure(result);
        }

        return Results.Ok(result);
    }
}