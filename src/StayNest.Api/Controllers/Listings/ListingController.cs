using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayNest.Api.Abstractions;
using StayNest.Api.Controllers.Listings.Requests;
using StayNest.Application.Abstractions.PageSize;
using StayNest.Application.Listings.Commands.AddFavorite;
using StayNest.Application.Listings.Commands.AddListing;
using StayNest.Application.Listings.Commands.CancelReservation;
using StayNest.Application.Listings.Commands.DeleteListing;
using StayNest.Application.Listings.Commands.RemoveFavorite;
using StayNest.Application.Listings.Commands.RemoveListingImage;
using StayNest.Application.Listings.Commands.Reserve;
using StayNest.Application.Listings.Commands.UpdateListing;
using StayNest.Application.Listings.Commands.UploadListingImage;
using StayNest.Application.Listings.Queries.GetListing;
using StayNest.Application.Listings.Queries.GetListingById;
using StayNest.Application.Listings.Queries.GetListingImage;
using StayNest.Application.Listings.Queries.GetMyListingById;
using StayNest.Application.Listings.Queries.GetReservations;
using StayNest.Application.Listings.Queries.GetUserFavorites;
using StayNest.Application.Listings.Responses;
using StayNest.Infrastructure.Authorization;
using StayNest.SharedKernel.Helpers;

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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetListing(
        [FromQuery] QueryObject queryObject,
        CancellationToken cancellationToken)
    {
        var query = new GetListingsQuery(queryObject);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<PaginatedResult<ListingResponse>>(
                result.Value,
                "Listing retrieved successfully"
            )) : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetListingById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetListingByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<ListingResponse>(
                result.Value,
                "Fetching listing by Id is successfull."
            )) : HandleFailure(result);
    }

    [HttpGet("my/{id:guid}")]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetMyListingById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetMyListingByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<ListingResponse>(
                result.Value,
                "Fetching my listing by Id is successfull."
            )) : HandleFailure(result);
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
    [AllowAnonymous]
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

    [HttpGet("favorites")]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetUserFavorites(
        CancellationToken cancellationToken)
    {
        var query = new GetUserFavoritesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<IList<FavoriteResponse>>(
                result.Value,
                "User favorites retrieved successfully"))
            : HandleFailure(result);
    }

    [HttpPost("{id:guid}/favorites")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> AddFavorite(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new AddFavoriteCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse( "Listing added to favorites successfully"))
            : HandleFailure(result);
    }

    [HttpDelete("{id:guid}/favorites")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> RemoveFavorite(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFavoriteCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Listing removed from favorites successfully"))
            : HandleFailure(result);
    }

    [HttpGet("reservations")]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetReservations(
        [FromQuery] QueryObject queryObject,
        CancellationToken cancellationToken)
    {
        var query = new GetReservationsQuery(queryObject);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse<PaginatedResult<ReservationResponse>>(
                result.Value,
                "User reservations retrieved successfully"))
            : HandleFailure(result);
    }

    [HttpPost("{id:guid}/reserve")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> ReserveListing(
        [FromRoute] Guid id,
        [FromBody] ReserveRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveCommand(
            id,
            request.StartDate,
            request.EndDate);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Listing reserved successfully"))
            : HandleFailure(result);
    }

    [HttpPost("{id:guid}/cancel-reservation")]
    [HasPermission(Permissions.UsersWrite)]
    public async Task<IActionResult> CancelReservation(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CancelReservationCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new ApiResponse("Reservation canceled successfully"))
            : HandleFailure(result);
    }
}
