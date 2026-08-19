using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayNest.Domain.Reservations.Entities;

namespace StayNest.Infrastructure.Configurations;

internal sealed class ReservationConfiguration
    : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");

        builder.HasKey(reservation => reservation.Id);

        builder.ComplexProperty(reservation => reservation.Period, period =>
        {
            period.Property(p => p.StartDate)
                .IsRequired();

            period.Property(p => p.EndDate)
                .IsRequired();
        });

        builder.OwnsOne(reservation => reservation.TotalPrice, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("total_price")
                .IsRequired();
        });

        builder.HasOne(reservation => reservation.User)
            .WithMany(user => user.Reservations)
            .HasForeignKey(reservation => reservation.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(reservation => reservation.Listing)
            .WithMany(listing => listing.Reservations)
            .HasForeignKey(reservation => reservation.ListingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}