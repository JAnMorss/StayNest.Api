namespace StayNest.Api.Controllers.Users.Requests;

public sealed record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password
);
