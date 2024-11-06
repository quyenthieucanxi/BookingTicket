using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Abstractions;

public interface ISeatRepository : IRepository<Seat>
{
    Task<bool> CheckSeatNumberExist(string seatNumber, CancellationToken cancellationToken = default);
    Task<bool> CheckFlightExist(FlightId flightId, CancellationToken cancellationToken = default);

    Task<bool> IsAvailable(SeatId id, CancellationToken cancellationToken = default);
}