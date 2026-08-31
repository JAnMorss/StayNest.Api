using Microsoft.EntityFrameworkCore;
using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.Helpers;
using StayNest.SharedKernel.Repositories;

namespace StayNest.Infrastructure.Repositories.Base;

internal abstract class Repository<T> 
    : QueryHooks<T>, IRepository<T>
    where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;

    protected Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        await _context
            .Set<T>()
            .AddAsync(entity, cancellationToken);
    }

    public async Task<int> CountAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Set<T>()
            .Where(e => EF.Property<Guid>(e, "UserId") == userId)
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context
            .Set<T>()
            .CountAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _context.Set<T>().Remove(entity);
            return true;

        }

        return false;
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        QueryObject queryObject,
        CancellationToken cancellationToken = default
    )
    {
        var query = BuildQuery(_context, queryObject);
        query = ApplySorting(query, queryObject);

        int skip = (queryObject.Page - 1) * queryObject.PageSize;

        return await query
            .Skip(skip)
            .Take(queryObject.PageSize)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var query = BuildQuery(_context, new QueryObject());

        return await query
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        _context.Set<T>().Update(entity);

        return Task.CompletedTask;
    }
}