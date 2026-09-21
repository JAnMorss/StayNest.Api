using StayNest.Application.Users.Responses;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Users.Queries.GetUserAvatar;

public sealed record GetUserAvatarQuery : IQuery<UserAvatarResponse>;