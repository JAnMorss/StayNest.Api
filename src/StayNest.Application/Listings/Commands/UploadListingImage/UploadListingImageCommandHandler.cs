using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Abstractions.BlobStorage;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Shared;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.UploadListingImage;

public sealed class UploadListingImageCommandHandler
    : ICommandHandler<UploadListingImageCommand>
{
    private const string Container = "listing-photos";

    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IBlobService _blobService;

    public UploadListingImageCommandHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IBlobService blobService)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _blobService = blobService;
    }
    public async Task<Result> Handle(
        UploadListingImageCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
            if (listing is null)
                return Result.Failure(ListingErrors.NotFound);

            if (listing.UserId != _userContext.UserId)
                return Result.Failure(ListingErrors.NotFound);

            var uploadedUrls = new List<string>();

            foreach (var file in request.Files)
            {
                if (!file.ContentType.StartsWith("image/"))
                {
                    return Result.Failure(ListingErrors.InvalidFile);
                }

                using var stream = file.OpenReadStream();

                var contentType = string.IsNullOrEmpty(file.ContentType)
                    ? "application/octet-stream"
                    : file.ContentType;

                var fileId = await _blobService.UploadAsync(
                    Container,
                    stream,
                    contentType,
                    cancellationToken);

                var url = _blobService.GetBlobUri(Container, fileId);

                uploadedUrls.Add(url);
            }

            var result = listing.AddImages(uploadedUrls);
            if (result.IsFailure)
                return Result.Failure(result.Error);

            await _listingRepository.UpdateAsync(listing, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Upload.Error", ex.Message));
        }
    }
}
