using MediatR;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.SharedKernel.Messaging.Query;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
