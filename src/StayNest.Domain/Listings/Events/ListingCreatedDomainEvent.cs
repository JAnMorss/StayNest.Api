using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Listings.Events;

public sealed record ListingCreatedDomainEvent(Guid Id) : IDomainEvent;