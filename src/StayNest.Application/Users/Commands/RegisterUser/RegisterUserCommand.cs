using StayNest.Application.Users.Responses;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : ICommand<UserResponse>;