using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.CancelReservation;

public sealed record CancelReservationCommand(Guid ReservationId) : ICommand;