using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.UpdateListing;

public sealed record UpdateListingCommand(
    Guid Id,
    string Title,
    string Description,
    string Category,
    int RoomCount,
    int BathroomCount,
    int GuestCount,
    string LocationValue,
    decimal Price) : ICommand<ListingResponse>;
