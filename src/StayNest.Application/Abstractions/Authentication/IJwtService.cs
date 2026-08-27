using StayNest.Domain.Users.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Application.Abstractions.Authentication;

public interface IJwtService
{
    Task<Result<string>> GetAccessTokenAsync(Email email, string password, CancellationToken cancellationToken = default);
}
