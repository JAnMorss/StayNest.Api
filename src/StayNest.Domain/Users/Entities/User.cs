using StayNest.Domain.Favorites.Entities;
using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.ValueObjects;
using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Users.Entities;

public sealed class User : BaseEntity
{
    private readonly List<RefreshToken> _refreshTokens = new();
    private readonly List<Listing> _listings = new();
    private readonly List<Reservation> _reservations = new();
    private readonly List<Favorite> _favorites = new();

    private User() { }
    private User(
        Guid id, 
        Name? name,
        Email? email, 
        DateTime? emailVerified,
        Image? image,
        HashedPassword? hashedPassword
    ) : base(id)
    {
        Name = name;
        Email = email;
        EmailVerified = emailVerified;
        Image = image;
        HashedPassword = hashedPassword;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public Name? Name {  get; private set; } 
    public Email? Email { get; private set; }
    public DateTime? EmailVerified { get; private set; }
    public Image? Image { get; private set; }
    public HashedPassword? HashedPassword { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens 
        => _refreshTokens.AsReadOnly();

    public IReadOnlyCollection<Favorite> Favorites
        => _favorites.AsReadOnly();

    public IReadOnlyCollection<Listing> Listings
        => _listings.AsReadOnly();

    public IReadOnlyCollection<Reservation> Reservations
        => _reservations.AsReadOnly();
}
