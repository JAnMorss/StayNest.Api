namespace StayNest.Api.Controllers.Users.Requests;

public sealed record RegisterUserRequest(
    string Name,
    string Image,
    string Email,
    string Password
);
