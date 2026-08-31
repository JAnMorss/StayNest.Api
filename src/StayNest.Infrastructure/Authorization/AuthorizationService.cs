using Microsoft.EntityFrameworkCore;
using StayNest.Domain.Users.Entities;

namespace StayNest.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorizationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var roles = await _dbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)
            .Select(u => new UserRolesResponse
            {
                UserId = u.Id,
                Roles = u.Roles.ToList()
            })
            .FirstAsync();

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var permissions = await _dbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.Permissions)
            .Select(p => p.Name)
            .ToListAsync();

        return permissions.ToHashSet();
    }

}
