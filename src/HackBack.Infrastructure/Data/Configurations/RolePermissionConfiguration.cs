using ZazaTemplate.Domain;
using ZazaTemplate.Infrastructure.Data.Entities;
using ZazaTemplate.Infrastructure.ServiceRegistration.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZazaTemplate.Infrastructure.Data.Configurations;

public class RolePermissionConfiguration(AuthorizationOptions authorizationOptions) : IEntityTypeConfiguration<RolePermissionEntity>
{
    private readonly AuthorizationOptions authorizationOptions = authorizationOptions;

    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        var data = authorizationOptions.Permissions
            .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermissionEntity()
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p)
                })).ToArray();
        builder.HasData(data);
    }
}
