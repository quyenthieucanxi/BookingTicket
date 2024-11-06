using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Abstractions;

public interface IBookingRepository : IRepository<Booking>
{
    Task<bool> CheckUserExist(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> CheckFlightExist(FlightId flightId, CancellationToken cancellationToken = default);

}