using Domain.Abstractions;
using Domain.Entities;
using Persistence.Data;

namespace Infrastructure.Repositories;

public class AirlineRepository : Repository<Airline>, IAirlineRepository
{
    public AirlineRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}