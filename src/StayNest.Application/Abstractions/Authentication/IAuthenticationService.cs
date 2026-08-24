using StayNest.Domain.Users.Entities;

namespace StayNest.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);
}
