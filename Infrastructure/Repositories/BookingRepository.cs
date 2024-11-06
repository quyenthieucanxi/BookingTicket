using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private readonly IUserRepository _userRepository;
    private readonly IFlightRepository _flightRepository;
    public BookingRepository(ApplicationDbContext applicationDbContext, 
        IUserRepository userRepository, 
        IFlightRepository flightRepository) : base(applicationDbContext)
    {
        _userRepository = userRepository;
        _flightRepository = flightRepository;
    }

    public async Task<bool> CheckUserExist(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.
            FindAsync(b => b.Id == userId.Value,cancellationToken) is not null;
    }

    public async Task<bool> CheckFlightExist(FlightId flightId, CancellationToken cancellationToken = default)
    {
        return await _flightRepository.
            FindAsync(b => b.Id == flightId,cancellationToken) is not null;

    }
}