using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Abstractions;

public interface IFlightRepository : IRepository<Flight>
{
    Task<Flight?> GetByIdWithSeats(FlightId id, CancellationToken cancellationToken = default);
                        
    Task<bool> CheckFlightNumberExist(string flightNumber, CancellationToken cancellationToken = default);
    Task<bool> CheckOriginAirportExist(AirportId originAirportId, CancellationToken cancellationToken = default);
    Task<bool> CheckDestinationAirportExist(AirportId destinationAirportId, CancellationToken cancellationToken = default);
    Task<bool> CheckAirlineExist(AirlineId airlineId, CancellationToken cancellationToken = default);

    
}