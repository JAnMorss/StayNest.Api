using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.Reserve;

public sealed record ReserveCommand(
    Guid ListingId,
    DateTime StartDate,
    DateTime EndDate) : ICommand;
