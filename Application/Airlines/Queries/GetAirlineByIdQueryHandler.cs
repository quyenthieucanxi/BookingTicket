using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Airlines.Queries;

public sealed class GetAirlineByIdQueryHandler : IQueryHandler<GetAirlineByIdQuery,AirlineResponse>
{
    private readonly IAirlineRepository _airlineRepository;

    public GetAirlineByIdQueryHandler(IAirlineRepository airlineRepository)
    {
        _airlineRepository = airlineRepository;
    }

    public async Task<Result<AirlineResponse>> Handle(GetAirlineByIdQuery request, CancellationToken cancellationToken)
    {
        var airline = await _airlineRepository.GetByIdAsync(request.Id, cancellationToken);

        if (airline is null)
        {
            return Result.Failure<AirlineResponse>(DomainErrors.Airline.NotFound);
        }

        var airlineResponse = 
            new AirlineResponse(request.Id.Value, airline.Name, airline.Country, airline.IATACode);
        return airlineResponse;
    }
}