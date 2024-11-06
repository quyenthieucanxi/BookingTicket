using Application.Airlines.Queries;
using Application.Airports.Queries;
using Domain.Entities;

namespace Application.Flights.Queries;

public record FlightResponse(Guid Id,string FlightNumber, 
    DateTime DepartureTime, DateTime ArrivalTime, 
    decimal Price, int Duration,AirportResponse Origin,AirportResponse Destination,AirlineResponse Airline);