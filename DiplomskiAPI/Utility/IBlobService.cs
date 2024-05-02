using Azure.Storage.Blobs;

namespace DiplomskiAPI.Services
{
    public interface IBlobService
    {
        string GetBlob(string blobName, string containerName);

        Object GetAllBlobs(string containerName);
        Task<bool> DeleteBlob(string blobName, string containerName);
        string UploadBlob(string blobName, string containerName, IFormFile file);

    }
}
