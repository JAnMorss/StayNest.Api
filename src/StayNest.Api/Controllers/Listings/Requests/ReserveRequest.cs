namespace StayNest.Api.Controllers.Listings.Requests;

public record ReserveRequest(
    DateTime StartDate,
    DateTime EndDate);