using ZazaTemplate.Domain;
using ResultSharp.Core;
namespace ZazaTemplate.Application.Abstractions.Services;

public interface IPermissionService
{
    Task<Result> HasPermissionAsync(Guid userId, Permission[] permissions, CancellationToken cancellationToken);
}
