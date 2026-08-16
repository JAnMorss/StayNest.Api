using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Users.Events;

public sealed record UserVerifyEmailDomainEvent(Guid Id) : IDomainEvent;