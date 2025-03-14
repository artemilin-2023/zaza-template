using HackBack.Domain;
using ResultSharp.Core;
namespace HackBack.Application.Abstractions.Services;

public interface IPermissionService
{
    Task<Result> HasPermissionAsync(Guid userId, Permission[] permissions, CancellationToken cancellationToken);
}
