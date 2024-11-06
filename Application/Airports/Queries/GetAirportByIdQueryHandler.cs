using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Airports.Queries;

public sealed class GetAirportByIdQueryHandler : IQueryHandler<GetAirportByIdQuery,AirportResponse>
{
    private readonly IAirportRepository _airportRepository;

    public GetAirportByIdQueryHandler(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<Result<AirportResponse>> Handle(GetAirportByIdQuery request, CancellationToken cancellationToken)
    {
        var airport = await _airportRepository.GetByIdAsync(request.Id, cancellationToken);

        if (airport is null)
        {
            return Result.Failure<AirportResponse>(DomainErrors.Airport.NotFound);
        }

        var airportResponse = new AirportResponse(airport.Id.Value, airport.Name, airport.Location, airport.Code);
        return airportResponse;
    }
}