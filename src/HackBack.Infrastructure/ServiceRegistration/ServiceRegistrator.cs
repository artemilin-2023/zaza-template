using ZazaTemplate.Application.Abstractions.Auth;
using ZazaTemplate.Application.Abstractions.Data;
using ZazaTemplate.Infrastructure.Auth;
using ZazaTemplate.Infrastructure.Data;
using ZazaTemplate.Infrastructure.Data.Repositories;
using ZazaTemplate.Infrastructure.ServiceRegistration.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZazaTemplate.Application.Abstractions.Auth;

namespace ZazaTemplate.Infrastructure.ServiceRegistration;

public static class ServiceRegistrator
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddRedisCache(configuration)
            .LoadOptions(configuration)
            .AddRepositories()
            .AddAuth();

        return services;
    }

    public static IServiceCollection LoadOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PasswordManagerOptions>(
            configuration.GetSection(nameof(PasswordManagerOptions)));

        services.Configure<Options.AuthorizationOptions>(
            configuration.GetSection(nameof(Options.AuthorizationOptions)));

        services.Configure<JwtOptions>(
            configuration.GetSection(nameof(JwtOptions)));

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        return services;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Redis options are not configured");

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<ITokenStorage, TokenStorage>()
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}