using ZazaTemplate.Domain;
using ZazaTemplate.Infrastructure.Data.Entities;

namespace ZazaTemplate.Infrastructure.Data.Mappers;

public static class UserEntityMapper
{
    public static User Map(this UserEntity entity)
        => new
        (
            entity.Id,
            entity.Name,
            entity.Password,
            entity.Email,
            [.. entity.Roles.Select(r => Enum.Parse<Role>(r.Name))]
        );

    public static UserEntity Map(this User user)
        => new()
        {
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Password = user.Password
        };
}

