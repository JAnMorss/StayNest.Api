using StayNest.Application.Abstractions.Authentication;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Shared;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.AddFavorite;

public sealed class AddFavoriteCommandHandler
    : ICommandHandler<AddFavoriteCommand>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddFavoriteCommandHandler(
        IListingRepository listingRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        AddFavoriteCommand request, 
        CancellationToken cancellationToken)
    {
        await _listingRepository.AddFavoriteAsync(
            _userContext.UserId,
            request.ListingId, 
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
