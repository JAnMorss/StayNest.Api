using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Reservations.Interface;
using StayNest.Infrastructure.Repositories.Base;

namespace StayNest.Infrastructure.Repositories;

internal sealed class ReservationRepository
    : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
