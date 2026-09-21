using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StayNest.Application.Abstractions.BlobStorage;

namespace StayNest.Infrastructure.BlobStorage;

internal sealed class BlobService(BlobServiceClient blobServiceClient) : IBlobService
{
    public async Task DeleteAsync(
        string container,
        Guid fileId,
        CancellationToken cancellationToken = default
    )
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<FileResponse> DownloadAsync(
        string container,
        Guid fileId,
        CancellationToken cancellationToken = default
    )
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse(
            response.Value.Content.ToStream(),
            response.Value.Details.ContentType ?? "application/octet-stream",
            fileId.ToString());
    }

    public string GetBlobUri(
        string container,
        Guid fileId
    )
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(container);

        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        return blobClient.Uri.ToString();
    }

    public async Task<Guid> UploadAsync(
        string container,
        Stream stream,
        string contentType,
        CancellationToken cancellationToken = default
    )
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);

        await containerClient.CreateIfNotExistsAsync(
            PublicAccessType.Blob,
            cancellationToken: cancellationToken);

        var fileId = Guid.NewGuid();

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: cancellationToken);

        return fileId;
    }
}
