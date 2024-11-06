using Application.Bookings.Commands;
using Carter;
using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Presentation.Abstractions;

namespace Presentation.Module;

public sealed class BookingModule : ModuleBase, ICarterModule
{
    private const string Tags = "Bookings";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/bookings/create", CreateBooking)
            .WithTags(Tags);
    }

    private async Task<IResult> CreateBooking(CreateBookingRequest request, ISender sender,CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(request.BookingDate,
            (BookingStatus)Enum.Parse(typeof(BookingStatus), request.Status,true), 
           new UserId(request.UserId), new FlightId(request.FlightId),
            request.Passengers,request.SeatIds.Select(guid => new SeatId(guid)).ToList());

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }
}