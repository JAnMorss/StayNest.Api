using StayNest.Domain.Users.Entities;
using StayNest.Domain.Users.ValueObjects;
using StayNest.SharedKernel.Repositories;

namespace StayNest.Domain.Users.Interface;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
}
