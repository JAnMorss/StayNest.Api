using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.RemoveFavorite;

public sealed record RemoveFavoriteCommand(Guid ListingId) : ICommand;
