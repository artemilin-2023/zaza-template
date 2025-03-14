using ZazaTemplate.Domain;
using Microsoft.AspNetCore.Authorization;

namespace ZazaTemplate.Infrastructure.Auth;

public class PermissionRequirement(params Permission[] permissions) :
    IAuthorizationRequirement
{
    public Permission[] Permissions { get; } = permissions;
}