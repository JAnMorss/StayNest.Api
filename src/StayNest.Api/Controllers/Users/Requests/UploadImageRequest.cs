namespace StayNest.Api.Controllers.Users.Requests;

public sealed class UploadImageRequest
{
    public IFormFile File { get; set; } = default!;
}
