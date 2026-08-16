using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Users.Events;

public sealed record UserUpdateDomainEvent(Guid Id) : IDomainEvent;