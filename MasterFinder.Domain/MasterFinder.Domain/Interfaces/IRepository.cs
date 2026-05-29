using MasterFinder.Domain.Base;

namespace MasterFinder.Domain.Interfaces
{
    public interface IRepository<TEntity, in TId>
        where TEntity : Entity<TId>
        where TId : struct, IEquatable<TId>
    {
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default);
    }
}