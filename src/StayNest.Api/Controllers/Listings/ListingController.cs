using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StayNest.Api.Abstractions;
using StayNest.Api.Controllers.Listings.Requests;
using StayNest.Application.Listings.Commands.AddListing;
using StayNest.Application.Listings.Responses;
using StayNest.Infrastructure.Authorization;

namespace StayNest.Api.Controllers.Listings;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/listing")]
public class ListingController : ApiController
{
    public ListingController(ISender sender) 
        : base(sender)
    {
    }

    [HttpPost]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> AddListing(
        [FromBody] ListingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddListingCommand(
            request.Title,
            request.Description,
            request.Category,
            request.RoomCount,
            request.BathroomCount,
            request.GuestCount,
            request.LocationValue,
            request.Price);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<ListingResponse>(
                result.Value,
                "Adding List successfully!")) 
            : HandleFailure(result);
    }
}
