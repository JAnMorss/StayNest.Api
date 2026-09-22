using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.AddListing;

public sealed record AddListingCommand(
    string Title,
    string Description,
    string Category,
    int RoomCount,
    int BathroomCount,
    int GuestCount,
    string LocationValue,
    decimal Price) : ICommand<ListingResponse>;
