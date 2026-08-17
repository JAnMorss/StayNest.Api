using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Users.Events;

public sealed record UserUpdateEmailDomainEvent(Guid UserId) : IDomainEvent;