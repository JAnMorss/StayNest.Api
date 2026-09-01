namespace StayNest.Domain.Users.Entities;

public sealed class RefreshToken
{
    public RefreshToken(
        Guid id, 
        Guid userId,
        string token, 
        DateTime expiryDate)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiryDate = expiryDate;
        isRevoked = false;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiryDate { get; private set; }
    public bool isRevoked { get; private set; }
    public User User { get; private set; } = null!;

    public bool IsExpired()
        => DateTime.UtcNow >= ExpiryDate;

    public void Revoke()
        => isRevoked = true;
}
