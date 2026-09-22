namespace StayNest.Api.Controllers.Listings.Requests;

public record ListingRequest(
    string Title,
    string Description,
    string Category,
    int RoomCount,
    int BathroomCount,
    int GuestCount,
    string LocationValue,
    decimal Price);
