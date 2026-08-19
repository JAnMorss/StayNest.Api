using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Listings.ValueObjects;

namespace StayNest.Infrastructure.Configurations;

internal sealed class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable("listings");

        builder.HasKey(listing => listing.Id);

        builder.OwnsOne(listing => listing.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("title")
                .HasMaxLength(Title.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.Description, description =>
        {
            description.Property(d => d.Value)
                .HasColumnName("description")
                .HasMaxLength(Description.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.ImageSrc, image =>
        {
            image.Property(i => i.Value)
                .HasColumnName("image_src")
                .HasMaxLength(500)
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.Category, category =>
        {
            category.Property(c => c.Value)
                .HasColumnName("category")
                .HasMaxLength(Category.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.RoomCount, roomCount =>
        {
            roomCount.Property(r => r.Value)
                .HasColumnName("room_count")
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.BathroomCount, bathroomCount =>
        {
            bathroomCount.Property(b => b.Value)
                .HasColumnName("bathroom_count")
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.GuestCount, guestCount =>
        {
            guestCount.Property(g => g.Value)
                .HasColumnName("guest_count")
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.LocationValue, location =>
        {
            location.Property(l => l.Value)
                .HasColumnName("location_value")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(listing => listing.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("price")
                .IsRequired();
        });

        builder.HasOne(listing => listing.User)
            .WithMany(user => user.Listings)
            .HasForeignKey(listing => listing.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(listing => listing.Reservations)
            .WithOne(reservation => reservation.Listing)
            .HasForeignKey(reservation => reservation.ListingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
