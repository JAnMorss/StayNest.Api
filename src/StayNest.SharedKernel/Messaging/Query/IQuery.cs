using MediatR;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.SharedKernel.Messaging.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
