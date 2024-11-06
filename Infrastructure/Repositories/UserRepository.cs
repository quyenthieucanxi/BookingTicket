using Domain.Abstractions;
using Domain.Entities;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return await FindAsync(u => u.Email == email,cancellationToken) is null;
    }
}