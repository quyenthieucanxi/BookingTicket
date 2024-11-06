using FluentValidation;

namespace Application.Bookings.Commands;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.BookingDate).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FlightId).NotEmpty();
    }
}