using StayNest.Domain.Listings.Entities;

namespace StayNest.Application.Listings.Responses;

public sealed class ListingImageResponse
{
    public List<string> ImageUrls { get; init; } = new();

    public static ListingImageResponse FromEntity(Listing listing)
    {
        return new ListingImageResponse
        {
            ImageUrls = listing.ImageSrc
                .Select(image => image.Value)
                .ToList()
        };
    }
}
