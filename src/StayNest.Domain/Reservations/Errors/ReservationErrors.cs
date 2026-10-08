using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Reservations.Errors;

public static class ReservationErrors
{
    public static readonly Error NotFound = new(
        "Reservation.NotFound",
        "The specified reservation was not found."
    );

    public static readonly Error AlreadyCancelled = new(
        "Reservation.AlreadyCancelled",
        "The reservation has already been cancelled."
    );
}
