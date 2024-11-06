using Domain.Entities;
using Domain.Enums;
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

        builder.Property(t => t.SeatId)
            .HasConversion(id => id.Value,
                id => new(id))
            .ValueGeneratedNever();

        builder.Property(t => t.Class)
            .HasConversion(
                t => t.ToString(),            
                t => (TicketClass)Enum.Parse(typeof(TicketClass),t) 
            );

        builder.Property(t => t.IssueDate)
            .IsRequired();

        builder.HasOne(t => t.Booking)
            .WithMany(b => b.Tickets)
            .HasForeignKey(t => t.BookingId);

        builder.HasOne(t => t.Seat)
            .WithOne()
            .HasForeignKey<Ticket>(t => t.SeatId);
        
    }
}