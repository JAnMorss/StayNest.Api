using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StayNest.Api.Abstractions;
using StayNest.Api.Controllers.Listings.Requests;
using StayNest.Application.Listings.Commands.AddListing;
using StayNest.Application.Listings.Commands.DeleteListing;
using StayNest.Application.Listings.Commands.RemoveListingImage;
using StayNest.Application.Listings.Commands.UpdateListing;
using StayNest.Application.Listings.Commands.UploadListingImage;
using StayNest.Application.Listings.Queries.GetListingImage;
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

    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> UpdateListing(
        [FromRoute] Guid id,
        [FromBody] ListingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateListingCommand(
            id,
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
                "Listing updated successfully"))
            : HandleFailure(result);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> DeleteListing(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteListingCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Listing deleted successfully"))
            : HandleFailure(result);
    }

    [HttpGet("{id:guid}/image")]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetListingImage(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetListingImageQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<ListingImageResponse>(
                result.Value,
                "Listing Image retrieved successfully"))
            : HandleFailure(result);
    }

    [HttpPost("{id:guid}/image")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> UploadListingImage(
        [FromRoute] Guid id,
        [FromForm] List<IFormFile> files,
        CancellationToken cancellationToken
    )
    {
        var command = new UploadListingImageCommand(id, files);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Liting image uploaded successfully"))
            : HandleFailure(result);
    }

    [HttpDelete("{id:guid}/image")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> DeleteListingImage(
        [FromRoute] Guid id,
        [FromQuery] string imageUrls,
        CancellationToken cancellationToken
    )
    {
        var command = new RemoveListingImageCommand(id, imageUrls);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Listing image deleted successfully"))
            : HandleFailure(result);
    }
}
