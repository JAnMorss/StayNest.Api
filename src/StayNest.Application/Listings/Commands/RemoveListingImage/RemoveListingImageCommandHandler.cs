using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Abstractions.BlobStorage;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Shared;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.RemoveListingImage;

public sealed class RemoveListingImageCommandHandler
    : ICommandHandler<RemoveListingImageCommand>
{
    private const string Container = "listing-photos";

    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IBlobService _blobService;

    public RemoveListingImageCommandHandler(
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
        RemoveListingImageCommand request, 
        CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
            return Result.Failure(ListingErrors.NotFound);

        if (listing.UserId != _userContext.UserId)
            return Result.Failure(ListingErrors.NotFound);

        if (!listing.ImageSrc.Any(i => i.Value == request.ImageUrl))
            return Result.Failure(ListingErrors.ImageNotFound);

        var fileId = Image.ExtractFileIdFromUrl(request.ImageUrl);
        if (!fileId.HasValue)
            return Result.Failure(ListingErrors.InvalidImage);

        await _blobService.DeleteAsync(
            Container,
            fileId.Value,
            cancellationToken);

        var result = listing.RemoveImages(request.ImageUrl);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _listingRepository.UpdateAsync(listing, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
