using Microsoft.EntityFrameworkCore;
using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Reservations.Interface;
using StayNest.Infrastructure.Repositories.Base;
using StayNest.SharedKernel.Helpers;

namespace StayNest.Infrastructure.Repositories;

internal sealed class ReservationRepository
    : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    protected override IQueryable<Reservation> BuildQuery(
        ApplicationDbContext context,
        QueryObject queryObject,
        Guid? userId = null)
    {
        var query = base.BuildQuery(context, queryObject, userId);

        if (userId.HasValue)
        {
            query = query.Where(e => e.UserId == userId.Value);
        }

        return query
            .Include(e => e.User)
            .Include(e => e.Listing);
    }

    protected override IQueryable<Reservation> ApplySorting(
        IQueryable<Reservation> query,
        QueryObject queryObject)
    {
        if (string.IsNullOrWhiteSpace(queryObject.SortBy))
            return query;

        query = queryObject.SortBy.ToLower() switch
        {
            "status" => queryObject.Descending
                ? query.OrderByDescending(r => r.Status)
                : query.OrderBy(r => r.Status),

            "createdAt" => queryObject.Descending
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),

            "updatedAt" => queryObject.Descending
                ? query.OrderByDescending(r => r.UpdatedAt)
                : query.OrderBy(r => r.UpdatedAt),

            _ => query
        };

        return query;
    }
}