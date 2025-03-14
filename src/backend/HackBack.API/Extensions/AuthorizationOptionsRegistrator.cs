using HackBack.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace HackBack.API.Extensions
{
    public static class AuthorizationOptionsRegistrator
    {
        public static IServiceCollection AddAuthorizationPermissionRequirements(this IServiceCollection services)
        {
            var controllerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.BaseType == typeof(ControllerBase));

            var fromMethods = controllerTypes.GetAttributesFromMethods();
            var fromClasses = controllerTypes.GetAttributesFromClasses();

            var authorizationAttributes = fromMethods.Concat(fromClasses)
                .GroupBy(attr => attr.Policy) // группируем по политике, где политика - строка в формате "Permission1 Permission2 Permission3"
                .Select(group => group.First()); // берем по одному аттрибуту на каждую политику, остальные - дубликаты

            var builder = services.AddAuthorizationBuilder();

            foreach (var attribute in authorizationAttributes)
            {
                builder.AddPolicy(attribute.Policy!, policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement(attribute!.Permissions));
                });
            }

            return services;
        }

        private static IEnumerable<PermissionRequiredAttribute> GetAttributesFromMethods(this IEnumerable<Type> controllerTypes)
            => controllerTypes
                .SelectMany(c => c.GetMethods())
                .FilterAttributes();

        private static IEnumerable<PermissionRequiredAttribute> FilterAttributes(this IEnumerable<MemberInfo> memberInfos)
            => memberInfos
                .Where(m => m.GetCustomAttribute<PermissionRequiredAttribute>(inherit: true) != null)
                .Select(m => m.GetCustomAttribute<PermissionRequiredAttribute>(inherit: true)!)
                ?? [];

        private static IEnumerable<PermissionRequiredAttribute> GetAttributesFromClasses(this IEnumerable<Type> controllerTypes)
            => controllerTypes.FilterAttributes();
    }
}
