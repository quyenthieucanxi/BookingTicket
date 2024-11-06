using System.Linq.Expressions;
using Application.Abstractions.Messaging;
using Application.Airlines.Queries;
using Application.Airports.Queries;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Flights.Queries;

public class GetFlightsQueryHandler : IQueryHandler<GetFlightsQuery,PageList<FlightResponse>>
{
    private readonly IFlightRepository _flightRepository;

    public GetFlightsQueryHandler(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    public async Task<Result<PageList<FlightResponse>>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {

        var flights =
            await _flightRepository.FindAllAsync(GetPropertySearch(request),
                 cancellationToken,null,null,
                 GetPropertySort(request),
                request.SortOrder?.ToUpper(),
                new string[]{nameof(Flight.Origin),nameof(Flight.Destination),nameof(Flight.Airline)});
        var flightsResponse =flights.Select(flight => new FlightResponse(
            flight.Id.Value,
            flight.FlightNumber,
            flight.DepartureTime,
            flight.ArrivalTime,
            flight.Price,
            flight.Duration,
            new AirportResponse(
                flight.OriginAirportId.Value,
                flight.Origin.Name,
                flight.Origin.Location,
                flight.Origin.Code
            ),
            new AirportResponse(
                flight.DestinationAirportId.Value,
                flight.Destination.Name,
                flight.Destination.Location,
                flight.Destination.Code
            ),
            new AirlineResponse(
                flight.AirlineId.Value,
                flight.Airline.Name,
                flight.Airline.Country,
                flight.Airline.IATACode
            )
        )).ToList();
        var pageListFlightsResponse =
            PageList<FlightResponse>.Create(flightsResponse, request.Page, request.PageSize);

        return pageListFlightsResponse;
    }

    private static Expression<Func<Flight, bool>>? GetPropertySearch(GetFlightsQuery request)
    {
        if (string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            return null;
        }
        Expression<Func<Flight, bool>> keySearch = flight =>
            flight.Airline.IATACode.Contains(request.SearchTerm) ||
            flight.Airline.Name.Contains(request.SearchTerm);
        return keySearch;
    }

    private static Expression<Func<Flight, object>>? GetPropertySort(GetFlightsQuery request)
    {
        if (string.IsNullOrWhiteSpace(request.SortColumn))
        {
            return null;
        }
        Expression<Func<Flight, object>> keySelector = request.SortColumn.ToLower() switch
        {
            "price" => fligt => fligt.Price,
            "departureTime" => flght => flght.DepartureTime,
            _ => flght => flght.Airline.Name
        };
        return keySelector;
    }
}