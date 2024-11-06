using Domain.Abstractions;
using Domain.Entities;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class AirportRepository : Repository<Airport>, IAirportRepository
{
    public AirportRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        
    }
}