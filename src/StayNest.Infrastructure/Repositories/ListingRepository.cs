using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Listings.Interface;
using StayNest.Infrastructure.Repositories.Base;

namespace StayNest.Infrastructure.Repositories;

internal sealed class ListingRepository
    : Repository<Listing>, IListingRepository
{
    public ListingRepository(ApplicationDbContext context) 
        : base(context)
    {
    }
}
