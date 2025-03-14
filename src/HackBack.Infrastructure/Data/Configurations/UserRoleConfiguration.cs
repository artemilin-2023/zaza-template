using ZazaTemplate.Domain;
using ZazaTemplate.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZazaTemplate.Infrastructure.Data.Configurations;

public class UserRoleConfiguration :
    IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}
