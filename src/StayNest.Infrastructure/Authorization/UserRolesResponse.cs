using StayNest.Domain.Users.Entities;

namespace StayNest.Infrastructure.Authorization;

public class UserRolesResponse
{
    public Guid UserId { get; init; }

    public List<Role> Roles { get; init; } = [];
}