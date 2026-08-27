using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Users.Responses;
using StayNest.Domain.Users.Errors;
using StayNest.Domain.Users.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler
    : ICommandHandler<LoginUserCommand, AccessTokenResponse>
{
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<AccessTokenResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(
                emailResult.Error);
        }

        var result = await _jwtService.GetAccessTokenAsync(
            emailResult.Value,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(
                UserErrors.InvalidCredentials);
        }

        return Result.Success(
            new AccessTokenResponse(result.Value));
    }
}
