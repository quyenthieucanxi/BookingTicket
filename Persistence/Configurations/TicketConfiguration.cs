using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();
        
        builder.Property(t => t.BookingId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();
        
        builder.Property(t => t.SeatNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.Class)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.IssueDate)
            .IsRequired();

        builder.HasOne(t => t.Booking)
            .WithMany(b => b.Tickets)
            .HasForeignKey(t => t.BookingId);
    }
}