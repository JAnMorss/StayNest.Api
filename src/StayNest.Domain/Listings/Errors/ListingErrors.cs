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

    public static readonly Error ImageNotFound = new(
        "Listing.ImageNotFound",
        "No photos have been uploaded for this property yet."
    );

    public static readonly Error InvalidImage = new(
        "Listing.InvalidImage",
        "The specified image URL is invalid."
    );

    public static readonly Error ListingNotAvailable = new(
        "Listing.ListingNotAvailable",
        "The listing is not available for the selected dates."
    );

    public static readonly Error CannotReserveOwnListing = new(
        "Reservation.CannotReserveOwnListing",
        "You cannot reserve your own listing."
    );

}
