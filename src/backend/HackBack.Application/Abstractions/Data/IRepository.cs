using ResultSharp;
using ResultSharp.Core;

namespace HackBack.Application.Abstractions.Data
{
    public interface IRepository<TEntity, TId>
    {
        Task<Result<TEntity>> GetAsync(TId id, CancellationToken cancellationToken);
        Result<IQueryable<TEntity>> GetAll();
        Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<Result<IEnumerable<TEntity>>> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
