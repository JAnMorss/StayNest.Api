using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.NotFound",
        "The user with the specified identifier was not found.");

    public static readonly Error InvalidEmail = new(
        "User.InvalidEmail",
        "The user's email address is invalid.");

    public static readonly Error EmailAlreadyExists = new(
        "User.EmailAlreadyExists",
        "The email address is already in use.");

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials are incorrect.");

    public static readonly Error InvalidPassword = new(
        "User.InvalidPassword",
        "The provided password is invalid.");

    public static readonly Error NoChangesDetected = new(
        "User.NoChangesDetected",
        "No changes were detected in the user's details."
    );

    public static readonly Error EmailAlreadyVerified = new(
        "User.EmailAlreadyVerified",
        "The user's email address is already verified.");

    public static readonly Error EmailNotFound = new(
        "User.EmailNotFound",
        "The user does not have an email address.");

    public static readonly Error InvalidImage = new(
        "User.InvalidImage",
        "The user's image URL is invalid.");

    public static readonly Error ImageNotFound = new(
        "User.ImageNotFound",
        "The user does not have an image.");

    public static readonly Error FavoriteAlreadyExists = new(
        "User.FavoriteAlreadyExists",
        "The listing has already been added to the user's favorites.");

    public static readonly Error FavoriteNotFound = new(
        "User.FavoriteNotFound",
        "The listing was not found in the user's favorites.");

    public static readonly Error RefreshTokenNotFound = new(
        "User.RefreshTokenNotFound",
        "The specified refresh token was not found.");

    public static readonly Error RefreshTokenExpired = new(
        "User.RefreshTokenExpired",
        "The refresh token has expired.");

    public static readonly Error RefreshTokenRevoked = new(
        "User.RefreshTokenRevoked",
        "The refresh token has been revoked.");

}