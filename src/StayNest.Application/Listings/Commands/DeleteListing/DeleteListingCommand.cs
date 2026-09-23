using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.DeleteListing;

public sealed record DeleteListingCommand(Guid ListingId) : ICommand;
