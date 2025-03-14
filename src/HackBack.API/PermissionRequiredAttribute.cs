using ZazaTemplate.Domain;
using Microsoft.AspNetCore.Authorization;

namespace ZazaTemplate.API
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
