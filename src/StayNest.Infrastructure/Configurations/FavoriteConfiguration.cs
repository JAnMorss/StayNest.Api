using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayNest.Domain.Favorites.Entities;

namespace StayNest.Infrastructure.Configurations;

internal sealed class FavoriteConfiguration
    : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("favorites");

        builder.HasKey(favorite => new
        {
            favorite.UserId,
            favorite.ListingId
        });

        builder.HasOne(favorite => favorite.User)
            .WithMany(user => user.Favorites)
            .HasForeignKey(favorite => favorite.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(favorite => favorite.Listing)
            .WithMany(listing => listing.Favorites)
            .HasForeignKey(favorite => favorite.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
