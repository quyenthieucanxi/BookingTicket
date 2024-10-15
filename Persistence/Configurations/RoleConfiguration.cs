using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasData(new Role() { Id = Guid.NewGuid(), Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" });
        builder.HasData(new Role() { Id = Guid.NewGuid(), Name = "User", ConcurrencyStamp = "5", NormalizedName = "USER" });
    }
}