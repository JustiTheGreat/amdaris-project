using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Infrastructure.Options;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AmdarisProject.Infrastructure.Persistance.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _profilePictureBlobContainerName;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IOptions<BlobStorageSettings> options)
        {
            BlobStorageSettings blobStorageSettings = options.Value;
            _profilePictureBlobContainerName = blobStorageSettings.ProfilePictureBlobContainerName;
            var credential = new StorageSharedKeyCredential(blobStorageSettings.StorageAccount, blobStorageSettings.AccessKey);
            var blobUri = $"https://{blobStorageSettings.StorageAccount}.blob.core.windows.net";
            _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string[] acceptableFileExtensions = ["jpeg", "png"];
            var fileExtension = file.FileName.Split('.').Last();

            if (!acceptableFileExtensions.Contains(fileExtension))
                throw new APException("Unsuported profile picture file format!");

            var filePath = $"{Guid.NewGuid()}.{fileExtension}";

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_profilePictureBlobContainerName);
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.UploadBlobAsync(filePath, stream);

            var blobUri = blobContainer.GetBlobClient(filePath).Uri.ToString();
            return blobUri;
        }

        public async Task<string> UpdateFile(string fileName, IFormFile file)
        {
            string[] acceptableFileExtensions = ["jpeg", "png"];
            var fileExtension = file.FileName.Split('.').Last();

            if (!acceptableFileExtensions.Contains(fileExtension))
                throw new APException("Unsuported profile picture file format!");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_profilePictureBlobContainerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            blobClient.Upload(stream, true);
            return blobClient.Uri.ToString();
        }

        public async Task DeleteFile(string fileName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_profilePictureBlobContainerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }
    }
}
