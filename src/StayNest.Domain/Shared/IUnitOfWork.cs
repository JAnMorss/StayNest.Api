using StayNest.Domain.Users.Entities;

namespace StayNest.Domain.Shared;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
