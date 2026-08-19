using StayNest.Domain.Reservations.Entities;
using StayNest.SharedKernel.Repositories;

namespace StayNest.Domain.Reservations.Interface;

public interface IReservationRepository : IRepository<Reservation>
{
}
