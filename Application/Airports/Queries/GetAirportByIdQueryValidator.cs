using FluentValidation;

namespace Application.Airports.Queries;

public class GetAirportByIdQueryValidator : AbstractValidator<GetAirportByIdQuery>
{
    public GetAirportByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}