using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Listings.ValueObjects;
using StayNest.Domain.Shared;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.AddListing;

public sealed class AddListingCommandHandler
    : ICommandHandler<AddListingCommand, ListingResponse>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public AddListingCommandHandler(
        IListingRepository listingRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork, 
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<ListingResponse>> Handle(
        AddListingCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        if (user is null)
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

        var listingResult = Listing.Create(
            user.Id,
            titleResult.Value,
            descriptionResult.Value,
            categoryResult.Value,
            roomCountResult.Value,
            bathroomCountResult.Value,
            guestCountResult.Value,
            locationValueResult.Value,
            priceResult.Value);

        if (listingResult.IsFailure)
            return Result.Failure<ListingResponse>(listingResult.Error);

        var listing = listingResult.Value;

        await _listingRepository.AddAsync(listing, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = ListingResponse.FromEntity(listing);

        return Result.Success(response);
    }
}
