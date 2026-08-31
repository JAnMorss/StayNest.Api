using StayNest.Application.Users.Responses;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Users.Queries.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;