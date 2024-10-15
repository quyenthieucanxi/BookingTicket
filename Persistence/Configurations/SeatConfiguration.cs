using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();
        
        builder.Property(s => s.FlightId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();

        builder.Property(s => s.SeatNumber)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(s => s.Class)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.IsAvailable)
            .IsRequired();

        builder.HasOne(s => s.Flight)
            .WithMany(f => f.Seats)
            .HasForeignKey(s => s.FlightId);
    }
}