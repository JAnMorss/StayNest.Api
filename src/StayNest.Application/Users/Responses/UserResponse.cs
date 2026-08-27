using StayNest.Domain.Users.Entities;
using System.ComponentModel.DataAnnotations;

namespace StayNest.Application.Users.Responses;

public sealed class UserResponse
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; } = string.Empty;
    public string? LastName { get; init; } = string.Empty;
    public string? Image { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    public string Role { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public static UserResponse FromEntity(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName?.Value,
            LastName = user.LastName?.Value,
            Image = user.Image?.Value,
            Email = user.Email?.Value,
            Role = user.Roles.Any()
                ? string.Join(", ", user.Roles.Select(r => r.Name))
                : string.Empty,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
