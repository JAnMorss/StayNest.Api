namespace StayNest.Api.Controllers.Users.Requests;

public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string Password
);
