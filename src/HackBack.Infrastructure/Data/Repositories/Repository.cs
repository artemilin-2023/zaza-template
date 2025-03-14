using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResultSharp.Core;
using ResultSharp.Errors;
using ResultSharp.Extensions.FunctionalExtensions.Async;
using ResultSharp.Logging;
using ZazaTemplate.Application.Abstractions.Data;

namespace ZazaTemplate.Infrastructure.Data.Repositories
{
    public class Repository<TEntity>(DataContext context, ILogger<Repository<TEntity>> logger) : IRepository<TEntity, Guid>
            where TEntity : class
    {
        private readonly DataContext context = context;
        private readonly DbSet<TEntity> dbSet = context.Set<TEntity>();
        private readonly ILogger<Repository<TEntity>> logger = logger;

        public async Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            // Не выглядит как хуйня ради хуйни?
            await Result.TryAsync(async () =>
            {
                await dbSet.AddAsync(entity, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                logger.LogDebug("Entity added: {Entity}", entity);
            }).LogErrorMessagesAsync();

            return entity;
        }

        public Task<Result<IEnumerable<TEntity>>> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            return Result.TryAsync(async () =>
            {
                var tasks = entities.Select((e) => dbSet.AddAsync(e, cancellationToken).AsTask());
                await Task.WhenAll(tasks);
                await context.SaveChangesAsync(cancellationToken);

                logger.LogDebug("{cnt} entities added", entities.Count());
                return entities;

            }, (ex) => Error.Failure($"Failed to add {entities.Count()} records for entity type {typeof(TEntity).Name}"))
            .LogErrorMessagesAsync();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            // тут для разнообразия дефолтный try/catch ^_^
            try
            {
                var entity = await dbSet.FindAsync([id], cancellationToken);
                if (entity == null)
                    return Error.NotFound();

                dbSet.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete entity with id {id}", id);
                return Error.Failure("Failed to delete entity");
            }
        }

        public Result<IQueryable<TEntity>> GetAll()
        {
            try
            {
                return Result.Success(dbSet.AsNoTracking().AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all entiteis");
                return Error.Failure("Failed to get entities");
            }
        }

        public Task<Result<TEntity>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return Result.TryAsync(
                () => dbSet.FindAsync([id], cancellationToken).AsTask(),
                ex => Error.Failure("Failure to find entity"))
            .EnsureAsync(entity => entity is not null)
            .MapAsync(e => e!)
            .LogErrorMessagesAsync();
        }

        public async Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                dbSet.Update(entity);
                await context.SaveChangesAsync(cancellationToken);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update entity {Entity}", entity);
                return Error.Failure("Failed to update entity");
            }
        }
    }
}
