using ZazaTemplate.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZazaTemplate.Infrastructure.Data.Configurations;

public class UserConfiguration :
    IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRoleEntity>(
                left => left.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                right => right.HasOne<UserEntity>().WithMany().HasForeignKey(u => u.UserId)
            );
    }
}
