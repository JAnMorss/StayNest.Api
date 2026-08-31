using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Users.Responses;
using StayNest.Domain.Shared;
using StayNest.Domain.Users.Entities;
using StayNest.Domain.Users.Interface;
using StayNest.Domain.Users.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticationService _authenticationService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAuthenticationService authenticationService
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
    }
    public async Task<Result<UserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);
        var emailResult = Email.Create(request.Email);
        var passwordHash = HashedPassword.FromPlainText(request.Password);

        Result validationResult = Result.FirstFailureOrSuccess(
            firstNameResult,
            lastNameResult,
            emailResult
        );

        if (validationResult.IsFailure)
            return Result.Failure<UserResponse>(validationResult.Error);

        var userResult = User.Create(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value,
            passwordHash
        );

        if (userResult.IsFailure)
            return Result.Failure<UserResponse>(userResult.Error);

        var user = userResult.Value;

        var identityId = await _authenticationService.RegisterAsync(
           user,
           request.Password,
           cancellationToken
        );

        user.SetIdentityId(identityId);

        await _userRepository.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(UserResponse.FromEntity(user));

    }
}
