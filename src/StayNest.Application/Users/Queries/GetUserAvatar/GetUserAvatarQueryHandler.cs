using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Abstractions.BlobStorage;
using StayNest.Application.Users.Responses;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Errors;
using StayNest.Domain.Users.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Users.Queries.GetUserAvatar;

public sealed class GetUserAvatarQueryHandler
    : IQueryHandler<GetUserAvatarQuery, UserAvatarResponse>
{
    private const string AvatarContainer = "avatar-photos";

    private readonly IUserRepository _userRepository;
    private readonly IBlobService _blobService;
    private readonly IUserContext _userContext;

    public GetUserAvatarQueryHandler(
        IUserRepository userRepository,
        IBlobService blobService,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _blobService = blobService;
        _userContext = userContext;
    }

    public async Task<Result<UserAvatarResponse>> Handle(
        GetUserAvatarQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            _userContext.UserId,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserAvatarResponse>(
                UserErrors.NotFound);
        }

        if (user.Image is null ||
            string.IsNullOrWhiteSpace(user.Image.Value))
        {
            return Result.Failure<UserAvatarResponse>(
                UserErrors.ImageNotFound);
        }

        var fileId = Image.ExtractFileIdFromUrl(user.Image.Value);

        if (!fileId.HasValue)
        {
            return Result.Failure<UserAvatarResponse>(
                UserErrors.InvalidImage);
        }

        var downloadResult = await _blobService.DownloadAsync(
            AvatarContainer,
            fileId.Value,
            cancellationToken);

        if (downloadResult is null)
        {
            return Result.Failure<UserAvatarResponse>(
                UserErrors.ImageNotFound);
        }

        using var memoryStream = new MemoryStream();

        await downloadResult.Stream.CopyToAsync(
            memoryStream,
            cancellationToken);

        var imageBytes = memoryStream.ToArray();

        var response = UserAvatarResponse.FromEntity(
            user,
            imageBytes,
            downloadResult.ContentType);

        return Result.Success(response);
    }
}
