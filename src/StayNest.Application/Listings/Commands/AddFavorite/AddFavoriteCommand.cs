using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.AddFavorite;

public sealed record AddFavoriteCommand(Guid ListingId) : ICommand;
