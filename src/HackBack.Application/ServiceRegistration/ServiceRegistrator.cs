using ZazaTemplate.Application.Abstractions.Services;
using ZazaTemplate.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using ZazaTemplate.Application.Abstractions.Services;

namespace ZazaTemplate.Application.ServiceRegistration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IPermissionService, PermissionService>();

            return services;
        }
    }
}
