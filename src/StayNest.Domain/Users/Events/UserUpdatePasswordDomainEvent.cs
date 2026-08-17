using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Users.Events;

public sealed record UserUpdatePasswordDomainEvent(Guid UserId) : IDomainEvent;