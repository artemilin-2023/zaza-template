using HackBack.Domain;
using ResultSharp.Core;

namespace HackBack.Application.Abstractions.Data;

public interface IUserRepository :
    IRepository<User, Guid>
{
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<HashSet<Permission>>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken);
}
