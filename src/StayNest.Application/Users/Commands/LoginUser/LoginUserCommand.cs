using StayNest.Application.Users.Responses;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password
) : ICommand<AccessTokenResponse>;