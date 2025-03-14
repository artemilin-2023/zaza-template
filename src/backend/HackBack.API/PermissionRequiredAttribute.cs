using HackBack.Domain;
using Microsoft.AspNetCore.Authorization;

namespace HackBack.API
{
    public class PermissionRequiredAttribute : AuthorizeAttribute
    {
        public Permission[] Permissions { get; }

        public PermissionRequiredAttribute(params Permission[] permissions)
        {
            Permissions = permissions;
            Policy = string.Join(" ", permissions.Order().Select(p => p.ToString()));
        }
    }
}
