namespace StayNest.Domain.Users.Entities;

public sealed class Role : IEquatable<Role>
{
    public static readonly Role Admin = new(1, "Admin");

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();

    public bool Equals(Role? other)
        => other is not null && Id == other.Id;

    public override bool Equals(object? obj)
        => obj is Role other && Equals(other);

    public override int GetHashCode()
        => Id.GetHashCode();
}
