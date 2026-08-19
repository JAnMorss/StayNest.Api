using Microsoft.EntityFrameworkCore;
using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.Helpers;

namespace StayNest.Infrastructure.Repositories.Base;

public abstract class QueryHooks<T> where T : BaseEntity
{
    protected virtual IQueryable<T> BuildQuery(
        ApplicationDbContext context,
        QueryObject queryObject,
        Guid? userId = null
    )
    {
        return context.Set<T>().AsQueryable();
    }

    protected virtual IQueryable<T> ApplyFilters(
        IQueryable<T> query,
        QueryObject queryObject,
        Guid? userId = null
    )
    {
        return query;
    }

    protected virtual IQueryable<T> ApplySorting(
        IQueryable<T> query,
        QueryObject queryObject
    )
    {
        if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
        {
            return queryObject.Descending
                ? query.OrderByDescending(e => EF.Property<object>(e, queryObject.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, queryObject.SortBy));
        }

        return query;
    }
}