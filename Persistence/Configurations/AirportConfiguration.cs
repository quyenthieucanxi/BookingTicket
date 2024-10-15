using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Location)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(10);
    }
}