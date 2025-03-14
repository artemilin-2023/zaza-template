using HackBack.Contracts.ApiContracts;
using HackBack.Domain;

namespace HackBack.API.Mappers
{
    public static class UserMapper
    {
        public static UserPublic Map(this User user)
            => new(user.Id, user.Username, user.Email, user.Roles.Select(r => r.ToString()));
    }
}
