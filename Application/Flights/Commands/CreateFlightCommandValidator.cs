using FluentValidation;

namespace Application.Flights.Commands;

public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        RuleFor(x => x.FlightNumber).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
        RuleFor(x => x.DepartureTime).NotEmpty();
        RuleFor(x => x.ArrivalTime).NotEmpty();
        RuleFor(x => x.OriginAirportId);
        RuleFor(x => x.DestinationAirportId);
        RuleFor(x => x.AirlineId).NotEmpty();
    }
}