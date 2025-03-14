using HackBack.Application.Abstractions;

namespace HackBack.Domain;

public class User(Guid id, string username, string password, string email, IReadOnlyList<Role> roles) :
    EntityObject<Guid>(id)
{
    public string Username { get; private set; } = username;
    public string Password { get; private set; } = password;
    public string Email { get; private set; } = email;
    public IReadOnlyList<Role> Roles { get; private set; } = roles;
}
