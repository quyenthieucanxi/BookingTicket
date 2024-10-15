using Domain.Entities;

namespace Domain.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<bool> IsEmailUniqueAsync(string email,
        CancellationToken cancellationToken = default);


}