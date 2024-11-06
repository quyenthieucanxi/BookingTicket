namespace Application.Flights.Commands;

public record CreateFlightRequest(string FlightNumber,DateTime DepartureTime,DateTime ArrivalTime,
    decimal Price,int Duration, Guid OriginAirportId,Guid DestinationAirportId,Guid AirlineId);