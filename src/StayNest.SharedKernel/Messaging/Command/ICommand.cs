using MediatR;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.SharedKernel.Messaging.Command;

public interface ICommand : IRequest<Result>, IBaseCommand { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand { }

public interface IBaseCommand { }
