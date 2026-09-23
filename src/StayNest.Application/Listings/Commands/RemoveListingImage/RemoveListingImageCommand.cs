using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.RemoveListingImage;

public sealed record RemoveListingImageCommand(
    Guid ListingId,
    string ImageUrl) : ICommand;
