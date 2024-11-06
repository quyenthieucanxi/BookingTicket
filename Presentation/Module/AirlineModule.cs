using Application.Airlines.Commands;
using Application.Airlines.Queries;
using Carter;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class AirlineModule : ModuleBase, ICarterModule
{
    private const string Tags = "Airlines";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/airlines/create",CreateAirline)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        
        app.MapGet("/airlines/{id}",GetAirlineById)
            .WithTags(Tags)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAirlineById(Guid id, ISender sender,CancellationToken cancellationToken)
    {
        var query = new GetAirlineByIdQuery(new AirlineId(id));
        Result<AirlineResponse> result = await sender.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result.Value);
    }

    private async Task<IResult> CreateAirline(CreateAirlineCommand request, 
        ISender sender,CancellationToken cancellationToken)
    {
        var command = new CreateAirlineCommand(request.Name,request.Country,request.IATACode);
        Result result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }
}