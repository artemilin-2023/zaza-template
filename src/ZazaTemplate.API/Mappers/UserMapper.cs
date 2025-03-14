using ZazaTemplate.Contracts.ApiContracts;
using ZazaTemplate.Domain;

namespace ZazaTemplate.API.Mappers
{
    public static class UserMapper
    {
        public static UserPublic Map(this User user)
            => new(user.Id, user.Username, user.Email, user.Roles.Select(r => r.ToString()));
    }
}
