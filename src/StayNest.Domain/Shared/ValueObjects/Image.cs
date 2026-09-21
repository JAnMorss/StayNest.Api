using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Shared.ValueObjects;

public sealed class Image : ValueObject
{
    private Image(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Image> Create(string image)
    {
        if (string.IsNullOrWhiteSpace(image))
        {
            return Result.Failure<Image>(new Error(
                "Image.Empty",
                "Image cannot be empty."));
        }

        if (!Uri.TryCreate(image, UriKind.RelativeOrAbsolute, out var uriResult))
        {
            return Result.Failure<Image>(new Error(
                "Image.InvalidUrl",
                "Your image URL is invalid."));
        }

        if (uriResult.IsAbsoluteUri &&
            uriResult.Scheme != Uri.UriSchemeHttp &&
            uriResult.Scheme != Uri.UriSchemeHttps)
        {
            return Result.Failure<Image>(new Error(
                "Image.InvalidUrl",
                "Your image URL is invalid."));
        }

        return Result.Success(new Image(image));
    }

    public static Guid? ExtractFileIdFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var fileName = Path.GetFileName(uri.LocalPath);

            return Guid.TryParse(fileName, out var fileId) ? fileId : null;
        }
        catch
        {
            return null;
        }
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
