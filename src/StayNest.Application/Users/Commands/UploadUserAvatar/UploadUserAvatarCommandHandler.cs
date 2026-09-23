using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Abstractions.BlobStorage;
using StayNest.Domain.Shared;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Errors;
using StayNest.Domain.Users.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Users.Commands.UploadUserAvatar;

public sealed class UploadUserAvatarCommandHandler
    : ICommandHandler<UploadUserAvatarCommand>
{
    private const string AvatarContainer = "avatar-photos";

    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IUserContext _userContext;

    public UploadUserAvatarCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IBlobService blobService,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        UploadUserAvatarCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if (user is null)
                return Result.Failure(UserErrors.NotFound);

            var existingFileId = user.Image is not null
                ? Image.ExtractFileIdFromUrl(user.Image.Value)
                : null;

            using var stream = request.AvatarFile.OpenReadStream();

            var contentType = string.IsNullOrWhiteSpace(
                request.AvatarFile.ContentType)
                ? "application/octet-stream"
                : request.AvatarFile.ContentType;

            var newFileId = await _blobService.UploadAsync(
                AvatarContainer,
                stream,
                contentType,
                cancellationToken);

            var newAvatarUrl = _blobService.GetBlobUri(
                AvatarContainer,
                newFileId);

            var avatarResult = user.UpdateImage(newAvatarUrl);

            if (avatarResult.IsFailure)
            {
                await _blobService.DeleteAsync(
                    AvatarContainer,
                    newFileId,
                    cancellationToken);

                return Result.Failure(avatarResult.Error);
            }

            await _userRepository.UpdateAsync(user, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (existingFileId.HasValue)
            {
                await _blobService.DeleteAsync(
                    AvatarContainer,
                    existingFileId.Value,
                    cancellationToken);
            }

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(
                new Error("Upload.Error", "An error occurred while uploading the avatar."));
        }
    }
}
