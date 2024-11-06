using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Flights.Commands;

public sealed record CreateFlightCommand(string FlightNumber,DateTime DepartureTime,DateTime ArrivalTime,
    decimal Price,int Duration, AirportId OriginAirportId,AirportId DestinationAirportId,AirlineId AirlineId  ) : ICommand;