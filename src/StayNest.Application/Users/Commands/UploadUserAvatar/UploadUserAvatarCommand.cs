using Microsoft.AspNetCore.Http;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Users.Commands.UploadUserAvatar;

public sealed record UploadUserAvatarCommand(IFormFile AvatarFile) : ICommand;
