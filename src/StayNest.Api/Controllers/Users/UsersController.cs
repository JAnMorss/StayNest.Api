using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayNest.Api.Abstractions;
using StayNest.Api.Controllers.Users.Requests;
using StayNest.Application.Users.Commands.LoginUser;
using StayNest.Application.Users.Commands.RegisterUser;
using StayNest.Application.Users.Responses;

namespace StayNest.Api.Controllers.Users;

[ApiController]
[Route("api/user")]
public class UsersController : ApiController
{
    public UsersController(ISender sender) 
        : base(sender)
    {
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
           ? Ok(new ApiResponse<UserResponse>(
               result.Value,
               "User registered successfully."
           )) : HandleFailure(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password
        );

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<AccessTokenResponse>(
                result.Value,
                "logged in successfully."
            )) : HandleFailure(result);
    }
}
