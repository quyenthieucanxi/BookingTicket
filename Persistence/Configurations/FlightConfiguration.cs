using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasKey(fl => fl.Id);
        
        builder.Property(fl => fl.Id)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();

        builder.Property(fl => fl.OriginAirportId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();

        builder.Property(fl => fl.DestinationAirportId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();

        builder.Property(fl => fl.AirlineId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();
        
        builder.Property(f => f.FlightNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(f => f.Bookings)
            .WithOne(b => b.Flight)
            .HasForeignKey(b => b.FlightId);

        builder.HasMany(f => f.Seats)
            .WithOne(s => s.Flight)
            .HasForeignKey(s => s.FlightId);

        builder.HasOne(fl => fl.Origin)
            .WithMany()
            .HasForeignKey(fl => fl.OriginAirportId);
        
        builder.HasOne(fl => fl.Destination)
            .WithMany()
            .HasForeignKey(fl => fl.DestinationAirportId);
        
        builder.HasOne(fl => fl.Airline)
            .WithMany(al => al.Flights)
            .HasForeignKey(fl => fl.AirlineId);
        
        
    }
}