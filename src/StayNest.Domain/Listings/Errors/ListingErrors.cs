using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.Errors;

public static class ListingErrors
{
    public static readonly Error NotFound = new(
        "Listing.NotFound",
        "The specified listing was not found."
    );

    public static readonly Error NoChangesDetected = new(
        "Listing.NoChangesDetected",
        "No changes detected in the property update."
    );

    public static readonly Error InvalidFile = new(
        "Listing.InvalidFile",
        "Only image and video files are allowed."
    );

    public static readonly Error ListingNotFound = new(
        "Listing.MediaNotFound",
        "No photos have been uploaded for this property yet."
    );

    public static readonly Error ListingMedia = new(
        "Listing.InvalidMedia",
        "The specified media URL is invalid."
    );

}
