using Domain.Entities;
using Domain.Primitives;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Configurations;
using Persistence.Outbox;

namespace Persistence.Data;

public sealed class ApplicationDbContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
{
    public new DbSet<Role> Roles { get; set; }
    public new  DbSet<User> Users { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    


    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new AirlineConfiguration());
        modelBuilder.ApplyConfiguration(new AirportConfiguration());
        modelBuilder.ApplyConfiguration(new FlightConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new PassengerConfiguration());
        modelBuilder.ApplyConfiguration(new SeatConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageCongfiguration());
    }

    // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    // {
    //     var domainEntities = ChangeTracker
    //         .Entries<IAggregateRoot>() 
    //         .Where(e => e.Entity.DomainEvents.Any())
    //         .Select(e => e.Entity)
    //         .ToList();
    //     
    //     var domainEvents = domainEntities
    //         .SelectMany(x => x.DomainEvents)
    //         .ToList();
    //
    //     var result = await base.SaveChangesAsync(cancellationToken);
    //     
    //     // Publish Event When Save Success
    //     foreach (var domainEvent in domainEvents)
    //     {
    //         await _publisher.Publish(domainEvent, cancellationToken);
    //     }
    //
    //     // Remove Domain 
    //     domainEntities.ForEach(entity => entity.ClearDomainEvents());
    //
    //     return result;
    // }
}