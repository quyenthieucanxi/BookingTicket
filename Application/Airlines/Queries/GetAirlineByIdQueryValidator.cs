using FluentValidation;

namespace Application.Airlines.Queries;

public class GetAirlineByIdQueryValidator : AbstractValidator<GetAirlineByIdQuery>
{
    public GetAirlineByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}