using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Users.Events;

public sealed record UserAvatarUpdatedDomainEvent(Guid Id) : IDomainEvent;
