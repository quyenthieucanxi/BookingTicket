using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class FlightRepository : Repository<Flight>, IFlightRepository
{
    private readonly IAirportRepository _airportRepository;
    private readonly IAirlineRepository _airlineRepository;
    public FlightRepository(ApplicationDbContext applicationDbContext, 
        IAirportRepository airportRepository, 
        IAirlineRepository airlineRepository) : base(applicationDbContext)
    {
        _airportRepository = airportRepository;
        _airlineRepository = airlineRepository;
    }

    public async Task<Flight?> GetByIdWithSeats(FlightId id, CancellationToken cancellationToken = default)
    {
        var flight = await FindAsync(f => f.Id == id, cancellationToken,
            new string[] {nameof(Flight.Seats) });
        return flight;
    }

    public async Task<bool> CheckFlightNumberExist(string flightNumber, CancellationToken cancellationToken = default)
    {
        return await FindAsync(fl => fl.FlightNumber == flightNumber,cancellationToken) is not null;
    }

    public async Task<bool> CheckOriginAirportExist(AirportId originAirportId, CancellationToken cancellationToken = default)
    {
        return await _airportRepository.
            FindAsync(fl => fl.Id == originAirportId,cancellationToken) is not null;
    }

    public async Task<bool> CheckDestinationAirportExist(AirportId destinationAirportId, CancellationToken cancellationToken = default)
    {
        return await _airportRepository.
            FindAsync(fl => fl.Id == destinationAirportId,cancellationToken) is not null;

    }

    public async Task<bool> CheckAirlineExist(AirlineId airlineId, CancellationToken cancellationToken = default)
    {
        return await _airlineRepository.
            FindAsync(fl => fl.Id == airlineId,cancellationToken) is not null;
    }
}