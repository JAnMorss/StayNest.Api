using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.Helpers;

namespace StayNest.SharedKernel.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(QueryObject queryObject, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Guid userId, CancellationToken cancellationToken = default);
}
