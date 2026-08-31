using Microsoft.EntityFrameworkCore;
using StayNest.Domain.Users.Entities;
using StayNest.Domain.Users.Interface;
using StayNest.Domain.Users.ValueObjects;
using StayNest.Infrastructure.Repositories.Base;
using StayNest.SharedKernel.Helpers;

namespace StayNest.Infrastructure.Repositories;

internal sealed class UserRepository
    : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public override async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        foreach (var role in user.Roles)
        {
            _context.Attach(role);
        }

        await base.AddAsync(user, cancellationToken);
    }

    protected override IQueryable<User> BuildQuery(
        ApplicationDbContext context,
        QueryObject queryObject,
        Guid? userId = null)
    {
        return context.Set<User>()
            .Include(u => u.Roles)
            .Include(u => u.Listings)
            .Include(u => u.Reservations)
            .Include(u => u.Favorites)
            .AsQueryable();
    }

    public Task<User?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(
            _context,
            new QueryObject());

        return query.FirstOrDefaultAsync(
            u => u.Email != null &&
                 u.Email.Value == email.Value,
            cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return _context.Set<User>()
            .AnyAsync(
                u => u.Email != null &&
                     u.Email.Value == email.Value,
                cancellationToken);
    }

    public Task<User?> GetByIdentityIdAsync(
        string identityId,
        CancellationToken cancellationToken = default)
    {
        return _context.Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(
                u => u.IdentityId == identityId,
                cancellationToken
            );
    }

}
