using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();
        
        builder.Property(p => p.BookingId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.PassportNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Nationality)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.DateOfBirth)
            .IsRequired();

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Passengers)
            .HasForeignKey(p => p.BookingId);
    }
}