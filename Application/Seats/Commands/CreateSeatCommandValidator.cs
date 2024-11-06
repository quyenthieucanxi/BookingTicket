using FluentValidation;

namespace Application.Seats.Commands;

public class CreateSeatCommandValidator : AbstractValidator<CreateSeatCommand>
{
    public CreateSeatCommandValidator()
    {
        RuleFor(x => x.SeatNumber).NotEmpty();
        RuleFor(x => x.Class).NotEmpty();
        RuleFor(x => x.IsAvailable).NotEmpty();
        RuleFor(x => x.FlightId).NotEmpty();
    }
}