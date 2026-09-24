using Microsoft.EntityFrameworkCore;
using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Listings.Interface;
using StayNest.Infrastructure.Repositories.Base;
using StayNest.SharedKernel.Helpers;

namespace StayNest.Infrastructure.Repositories;

internal sealed class ListingRepository
    : Repository<Listing>, IListingRepository
{
    public ListingRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    protected override IQueryable<Listing> BuildQuery(
        ApplicationDbContext context, 
        QueryObject queryObject, 
        Guid? userId = null)
    {
        var query = base.BuildQuery(context, queryObject, userId);

        if (userId.HasValue)
        {
            query = query.Where(e => e.UserId == userId.Value);
        }

        return query.Include(e => e.User);
    }

    protected override IQueryable<Listing> ApplySorting(
        IQueryable<Listing> query, 
        QueryObject queryObject)
    {
        if (string.IsNullOrWhiteSpace(queryObject.SortBy))
            return query;

        query = queryObject.SortBy.ToLower() switch
        {
            "title" => queryObject.Descending
                ? query.OrderByDescending(p => p.Title)
                : query.OrderBy(p => p.Title),

            "category" => queryObject.Descending
                ? query.OrderByDescending(p => p.Category)
                : query.OrderBy(p => p.Category),

            "roomCount" => queryObject.Descending
                ? query.OrderByDescending(p => p.RoomCount)
                : query.OrderBy(p => p.RoomCount),

            "bathroomCount" => queryObject.Descending
                ? query.OrderByDescending(p => p.BathroomCount)
                : query.OrderBy(p => p.BathroomCount),

            "guestCount" => queryObject.Descending
                ? query.OrderByDescending(p => p.GuestCount)
                : query.OrderBy(p => p.GuestCount),

            "locationValue" => queryObject.Descending
                ? query.OrderByDescending(p => p.LocationValue)
                : query.OrderBy(p => p.LocationValue),

            "price" => queryObject.Descending
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),

            _ => query
        };

        return query;
    }
}
