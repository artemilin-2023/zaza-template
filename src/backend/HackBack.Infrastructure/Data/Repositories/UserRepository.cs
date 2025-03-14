using HackBack.Application.Abstractions.Data;
using HackBack.Domain;
using HackBack.Infrastructure.Data.Contexts;
using HackBack.Infrastructure.Data.Entities;
using HackBack.Infrastructure.Data.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultSharp.Core;
using ResultSharp.Errors;

namespace HackBack.Infrastructure.Data.Repositories;

public class UserRepository(DataContext context, ILogger<UserRepository> logger) :
    IUserRepository
{
    private readonly DataContext context = context;
    private readonly ILogger<UserRepository> logger = logger;

    public async Task<Result<User>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Получение пользователя по ID: {UserId}", id);

        var entity = await context.Users
            .Include(u => u.Roles)
            .FirstAsync(u => u.Id == id, cancellationToken: cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("Пользователь с ID {UserId} не найден", id);
            return Error.NotFound($"Пользователь с ID {id} не найден");
        }

        logger.LogDebug("Пользователь с ID {UserId} успешно получен", id);
        return entity.Map();
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        logger.LogInformation("Получение пользователя по email: {Email}", email);

        var entity = await context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("Пользователь с email {Email} не найден", email);
            return Error.NotFound($"Пользователь с email {email} не найден");
        }

        logger.LogDebug("Пользователь с email {Email} успешно получен", email);
        return entity.Map();
    }

    public Result<IQueryable<User>> GetAll()
    {
        logger.LogInformation("Получение всех пользователей");

        return Result<IQueryable<User>>.Success(
            context.Users.Select(e => e.Map())
                .AsQueryable()
        );
    }

    public async Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken)
    {
        logger.LogInformation("Добавление нового пользователя с email: {Email}", user.Email);

        var entity = GetEntityWithRoles(user);
        await context.Users.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Пользователь успешно добавлен с ID: {UserId}", entity.Id);
        return entity.Map();
    }

    private UserEntity GetEntityWithRoles(User user)
    {
        var entity = user.Map();
        var roles = user.Roles.SelectMany(
            role => context.Roles.Where(roleEntity => roleEntity.Id == (int)role));
        entity.Roles.AddRange(roles);

        return entity;
    }

    public async Task<Result<IEnumerable<User>>> AddAsync(IEnumerable<User> users, CancellationToken cancellationToken)
    {
        logger.LogInformation("Добавление группы пользователей");

        var entities = users.Select(u => GetEntityWithRoles(u));
        await context.Users.AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Успешно добавлено {Count} пользователей", entities.Count());
        return Result<IEnumerable<User>>.Success(
            entities.Select(e => e.Map())
        );
    }

    public async Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обновление пользователя с ID: {UserId}", user.Id);

        var entity = user.Map();
        context.Users.Update(entity);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Пользователь с ID {UserId} успешно обновлен", user.Id);
        return entity.Map();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Удаление пользователя с ID: {UserId}", id);

        var entity = await context.Users.FindAsync(keyValues: [id], cancellationToken: cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("Пользователь с ID {UserId} не найден при попытке удаления", id);
            return Error.NotFound($"Пользователь с ID {id} не найден");
        }

        context.Users.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Пользователь с ID {UserId} успешно удален", id);
        return Result.Success();
    }

    public async Task<Result<HashSet<Permission>>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var roles = await context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToListAsync(cancellationToken);

        return roles
            .SelectMany(r => r)
            .SelectMany(p => p.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }
}
