using StayNest.Domain.Favorites.Entities;
using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Errors;
using StayNest.Domain.Users.Events;
using StayNest.Domain.Users.ValueObjects;
using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Users.Entities;

public sealed class User : BaseEntity
{
    private readonly List<Listing> _listings = new();
    private readonly List<Reservation> _reservations = new();
    private readonly List<Favorite> _favorites = new();
    private readonly List<Role> _roles = new();

    private User() { }
    private User(
        Guid id,
        FirstName? firstName,
        LastName? lastName,
        Email? email, 
        DateTime? emailVerified,
        Image? image,
        HashedPassword? hashedPassword
    ) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        EmailVerified = emailVerified;
        Image = image;
        HashedPassword = hashedPassword;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public FirstName? FirstName {  get; private set; }
    public LastName? LastName { get; private set; }
    public Email? Email { get; private set; }
    public DateTime? EmailVerified { get; private set; }
    public Image? Image { get; private set; }
    public HashedPassword? HashedPassword { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Role> Roles
        => _roles.AsReadOnly();
    public IReadOnlyCollection<Favorite> Favorites
        => _favorites.AsReadOnly();

    public IReadOnlyCollection<Listing> Listings
        => _listings.AsReadOnly();

    public IReadOnlyCollection<Reservation> Reservations
        => _reservations.AsReadOnly();

    public static Result<User> Create(
        FirstName? firstName,
        LastName? lastName,
        Email? email,
        HashedPassword? hashedPassword
    )
    {
        var user = new User(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            null,
            null,
            hashedPassword
        );

        user._roles.Add(Role.Registered);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return Result.Success(user);
    }

    public Result UpdateDetails(
        FirstName? firstName,
        LastName? lastName,
        Image? image
    )
    {
        if (Equals(FirstName, firstName) &&
            Equals(LastName, lastName) &&
            Equals(Image, image))
        {
            return Result.Failure(UserErrors.NoChangesDetected);
        }


        FirstName = firstName;
        LastName = lastName;
        Image = image;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserUpdateDomainEvent(Id));

        return Result.Success();
    }

    public Result UpdateEmail(Email email)
    {
        if (email is null)
            return Result.Failure(UserErrors.InvalidEmail);

        if (email == Email)
            return Result.Failure(UserErrors.NoChangesDetected);

        Email = email;

        EmailVerified = null;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserUpdateEmailDomainEvent(Id));

        return Result.Success();
    }

    public Result VerifyEmail()
    {
        if (Email is null)
            return Result.Failure(UserErrors.InvalidEmail);

        if (EmailVerified is not null)
            return Result.Failure(UserErrors.EmailAlreadyVerified);

        EmailVerified = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserVerifyEmailDomainEvent(Id));

        return Result.Success();
    }

    public Result UpdatePassword(HashedPassword hashedPassword)
    {
        if (hashedPassword is null)
            return Result.Failure(UserErrors.InvalidPassword);

        HashedPassword = hashedPassword;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserUpdatePasswordDomainEvent(Id));

        return Result.Success();
    }

    public void AddFavorite(Favorite favorite)
    {
        if (_favorites.Any(x => x.ListingId == favorite.ListingId))
            return;

        _favorites.Add(favorite);
    }


    public void RemoveFavorite(Guid listingId)
    {
        _favorites.RemoveAll(x => x.ListingId == listingId);
    }

    public void AddRole(Role role)
    {
        if (_roles.Any(r => r.Id == role.Id))
            return;

        _roles.Add(role);
    }

    public void RemoveRole(Role role)
    {
        _roles.RemoveAll(r => r.Id == role.Id);
    }

    public void ClearRoles()
        => _roles.Clear();

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}
