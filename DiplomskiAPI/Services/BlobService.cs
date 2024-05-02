using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DiplomskiAPI.Model;

namespace DiplomskiAPI.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }

        public async Task<bool> DeleteBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public object GetAllBlobs(string containerName)
        {
            BlobContainerClient bcc = _blobClient.GetBlobContainerClient(containerName);
            
            Azure.Pageable<BlobItem> b = bcc.GetBlobs();

            List<string> listOfBlobNames = new List<string>();
            foreach(var x in b)
            {
                BlobClient blobClient = bcc.GetBlobClient(x.Name);
                listOfBlobNames.Add(blobClient.Uri.AbsoluteUri);
            }
            return listOfBlobNames;
        }

        public string GetBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);


            return blobClient.Uri.AbsoluteUri;
        }

        public string UploadBlob(string blobName, string containerName, IFormFile file)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            var result = blobClient.Upload(file.OpenReadStream(), httpHeaders);
            if(result != null)
            {
                return GetBlob(blobName, containerName);
            }
            return "";
        }
    }
}
