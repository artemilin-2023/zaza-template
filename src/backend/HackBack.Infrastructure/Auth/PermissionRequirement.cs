using HackBack.Domain;
using Microsoft.AspNetCore.Authorization;

namespace HackBack.Infrastructure.Auth;

public class PermissionRequirement(params Permission[] permissions) :
    IAuthorizationRequirement
{
    public Permission[] Permissions { get; } = permissions;
}