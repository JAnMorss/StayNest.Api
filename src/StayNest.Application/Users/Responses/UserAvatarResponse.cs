using StayNest.Domain.Users.Entities;

namespace StayNest.Application.Users.Responses;

public sealed class UserAvatarResponse
{
    public string? AvatarUrl { get; init; }

    public byte[]? ImageBytes { get; init; }

    public string? ContentType { get; init; }

    public static UserAvatarResponse FromEntity(
        User user,
        byte[]? imageBytes = null,
        string? contentType = null)
    {
        return new UserAvatarResponse
        {
            AvatarUrl = user.Image?.Value,
            ImageBytes = imageBytes,
            ContentType = contentType
        };
    }
}
