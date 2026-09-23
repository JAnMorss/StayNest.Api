using Microsoft.AspNetCore.Http;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.UploadListingImage;

public sealed record UploadListingImageCommand(
    Guid ListingId,
    IEnumerable<IFormFile> Files) : ICommand;
