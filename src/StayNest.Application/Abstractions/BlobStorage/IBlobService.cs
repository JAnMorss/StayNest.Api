namespace StayNest.Application.Abstractions.BlobStorage;

public interface IBlobService
{
    Task<Guid> UploadAsync(string container, Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<FileResponse> DownloadAsync(string container, Guid fileId, CancellationToken cancellationToken = default);
    Task DeleteAsync(string container, Guid fileId, CancellationToken cancellationToken = default);
    string GetBlobUri(string container, Guid fileId);
}