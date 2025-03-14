using ZazaTemplate.Application.Abstractions.Data;
using ZazaTemplate.Application.Abstractions.Services;
using ZazaTemplate.Domain;
using ResultSharp.Core;
using ResultSharp.Errors;

namespace ZazaTemplate.Application.Services;

public class PermissionService(IUserRepository userRepository) : IPermissionService
{
    public async Task<Result> HasPermissionAsync(Guid userId, Permission[] permissions, CancellationToken cancellationToken)
    {
        var userPermissions = await userRepository.GetPermissionsAsync(userId, cancellationToken);
        if (userPermissions.IsFailure)
            return Result.Failure();

        return userPermissions.Value
            .Intersect(permissions).Any()
                ? Result.Success() 
                : Error.Forbidden();
    }
}

