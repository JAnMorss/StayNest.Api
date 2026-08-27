using StayNest.Domain.Users.Entities;

namespace StayNest.Infrastructure.Authentication.Models;

public sealed class UserRepresentationModel
{
    public Dictionary<string, string>? Access { get; set; }
    public Dictionary<string, List<string>>? Attributes { get; set; }
    public Dictionary<string, string>? ClientRoles { get; set; }
    public long? CreatedTimestamp { get; set; }
    public CredentialRepresentationModel[]? Credentials { get; set; }

    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
    public bool? Enabled { get; set; }

    public string? Id { get; set; }
    public string[]? Groups { get; set; }
    public int? NotBefore { get; set; }
    public string? Origin { get; set; }
    public string[]? RealmRoles { get; set; }
    public string[]? RequiredActions { get; set; }
    public string? Self { get; set; }
    public string? ServiceAccountClientId { get; set; }

    public string? Username { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    internal static UserRepresentationModel FromUser(User user) =>
        new()
        {
            Email = user.Email?.Value,
            Username = user.Email?.Value,

            FirstName = user.FirstName?.Value,
            LastName = user.LastName?.Value,

            Enabled = true,

            EmailVerified = user.EmailVerified is not null,

            CreatedTimestamp =
                new DateTimeOffset(user.CreatedAt)
                    .ToUnixTimeMilliseconds(),

            Attributes = new Dictionary<string, List<string>>
            {
                ["image"] = new()
                {
                    user.Image?.Value ?? string.Empty
                }
            },

            RequiredActions = Array.Empty<string>(),

            RealmRoles = user.Roles
                .Select(role => role.Name)
                .ToArray()
        };
}
