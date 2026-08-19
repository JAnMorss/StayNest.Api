using StayNest.Domain.Listings.Entities;
using StayNest.SharedKernel.Repositories;

namespace StayNest.Domain.Listings.Interface;

public interface IListingRepository : IRepository<Listing>
{
}
