using ZazaTemplate.Domain;
using ZazaTemplate.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZazaTemplate.Infrastructure.Data.Configurations;

public class RoleConfiguration :
    IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(r => r.Id);
        var roles = Enum
            .GetValues<Role>()
            .Select(r => new RoleEntity()
            {
                Id = (int)r,
                Name = r.ToString()
            })
            .ToList();
        
        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles);

        builder.HasData(roles);
    }
}
