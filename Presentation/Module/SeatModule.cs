using Application.Seats.Commands;
using Carter;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class SeatModule : ModuleBase, ICarterModule
{
    private const string Tags = "Seats";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/seats/create", CreateSeat)
            .WithTags(Tags);
    }

    private async Task<IResult> CreateSeat(CreateSeatRequest request, ISender sender,CancellationToken cancellationToken)
    {
        var command = new CreateSeatCommand(request.SeatNumber, request.Class, request.IsAvailable,
            new FlightId(request.FlightId));

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }
}