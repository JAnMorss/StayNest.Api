using StayNest.Domain.Reservations.Entities;

namespace StayNest.Application.Listings.Responses;

public sealed class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ListingId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ReservationResponse FromEntity(Reservation reservation)
    {
        return new ReservationResponse
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            ListingId = reservation.ListingId,

            StartDate = reservation.Period.StartDate,
            EndDate = reservation.Period.EndDate,

            TotalPrice = reservation.TotalPrice.Amount,
            Status = reservation.Status.ToString(),

            CreatedAt = reservation.CreatedAt
        };
    }
}