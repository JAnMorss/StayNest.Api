using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayNest.Domain.Users.Entities;
using StayNest.Domain.Users.ValueObjects;

namespace StayNest.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.OwnsOne(user => user.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("name")
                .HasMaxLength(Name.MaxLength)
                .IsRequired(false);
        });

        builder.OwnsOne(user => user.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired(false);

            email.HasIndex(e => e.Value)
                .IsUnique();
        });

        builder.OwnsOne(user => user.Image, image =>
        {
            image.Property(i => i.Value)
                .HasColumnName("image")
                .IsRequired(false);
        });

        builder.OwnsOne(user => user.HashedPassword, password =>
        {
            password.Property(p => p.Value)
                .HasColumnName("password_hash")
                .HasMaxLength(500)
                .IsRequired(false);
        });

        builder.Property(user => user.EmailVerified)
            .IsRequired(false);

        builder.HasIndex(user => user.IdentityId).IsUnique();

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired(false);

        builder.HasMany(user => user.Listings)
            .WithOne(listing => listing.User)
            .HasForeignKey(listing => listing.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(user => user.Reservations)
            .WithOne(reservation => reservation.User)
            .HasForeignKey(reservation => reservation.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(user => user.Favorites)
            .WithOne(favorite => favorite.User)
            .HasForeignKey(favorite => favorite.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
