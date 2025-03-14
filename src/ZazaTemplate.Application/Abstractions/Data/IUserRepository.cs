using ZazaTemplate.Domain;
using ResultSharp.Core;
using ZazaTemplate.Application.Abstractions.Data;

namespace ZazaTemplate.Application.Abstractions.Data;

public interface IUserRepository :
    IRepository<User, Guid>
{
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<HashSet<Permission>>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken);
}
