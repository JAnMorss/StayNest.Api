using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Users.Responses;
using StayNest.Domain.Users.Errors;
using StayNest.Domain.Users.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Users.Queries.GetLoggedInUser;

public sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public GetLoggedInUserQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(
        GetLoggedInUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdentityIdAsync(
            _userContext.IdentityId,
            cancellationToken);
        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound);

        var response = UserResponse.FromEntity(user);

        return Result.Success(response);
    }
}
