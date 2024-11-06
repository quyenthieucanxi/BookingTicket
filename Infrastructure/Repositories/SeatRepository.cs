using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class SeatRepository : Repository<Seat>, ISeatRepository
{
    private readonly IFlightRepository _flightRepository;
    public SeatRepository(ApplicationDbContext applicationDbContext, 
        IFlightRepository flightRepository) : base(applicationDbContext)
    {
        _flightRepository = flightRepository;
    }

    public async Task<bool> CheckSeatNumberExist(string seatNumber, CancellationToken cancellationToken = default)
    {
        return await FindAsync(s => s.SeatNumber == seatNumber, cancellationToken) is not null;
    }

    public async Task<bool> CheckFlightExist(FlightId flightId, CancellationToken cancellationToken = default)
    {
        return 
            await  _flightRepository.FindAsync(s => s.Id == flightId, cancellationToken) is not null;

    }

    public async Task<bool> IsAvailable(SeatId id, CancellationToken cancellationToken = default)
    {
        return await FindAsync(s => s.Id == id && s.IsAvailable, cancellationToken) is not null;
    }
}