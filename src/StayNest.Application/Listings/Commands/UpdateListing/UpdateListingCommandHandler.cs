using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Listings.ValueObjects;
using StayNest.Domain.Shared;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.UpdateListing;

public sealed class UpdateListingCommandHandler 
    : ICommandHandler<UpdateListingCommand, ListingResponse>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public UpdateListingCommandHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<ListingResponse>> Handle(
        UpdateListingCommand request,
        CancellationToken cancellationToken
    )
    {
        var listing = await _listingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (listing is null)
            return Result.Failure<ListingResponse>(ListingErrors.NotFound);

        if (listing.UserId != _userContext.UserId)
            return Result.Failure<ListingResponse>(ListingErrors.NotFound);

        var titleResult = Title.Create(request.Title);
        var descriptionResult = Description.Create(request.Description);
        var categoryResult = Category.Create(request.Category);
        var roomCountResult = RoomCount.Create(request.RoomCount);
        var bathroomCountResult = BathroomCount.Create(request.BathroomCount);
        var guestCountResult = GuestCount.Create(request.GuestCount);
        var locationValueResult = LocationValue.Create(request.LocationValue);
        var priceResult = Price.Create(request.Price);

        Result validationResult = Result.FirstFailureOrSuccess(
            titleResult,
            descriptionResult,
            categoryResult,
            roomCountResult,
            bathroomCountResult,
            guestCountResult,
            locationValueResult,
            priceResult);

        if (validationResult.IsFailure)
            return Result.Failure<ListingResponse>(validationResult.Error);

        var updateResult = listing.Update(
            titleResult.Value,
            descriptionResult.Value,
            categoryResult.Value,
            roomCountResult.Value,
            bathroomCountResult.Value,
            guestCountResult.Value,
            locationValueResult.Value,
            priceResult.Value);

        if (updateResult.IsFailure)
            return Result.Failure<ListingResponse>(updateResult.Error);

        await _listingRepository.UpdateAsync(listing, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = ListingResponse.FromEntity(listing);

        return Result.Success(response);
    }
}
