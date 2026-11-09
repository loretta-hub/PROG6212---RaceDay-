
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace RaceDay.API.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString =
                configuration["AzureBlobStorage:ConnectionString"]
                ?? throw new InvalidOperationException(
                    "Azure Blob Storage connection string is missing.");

            string containerName =
                configuration["AzureBlobStorage:ContainerName"]
                ?? "raceday-images";

            var blobServiceClient =
                new BlobServiceClient(connectionString);

            _containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadImageAsync(
            IFormFile file,
            string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Please select an image.");

            if (file.Length > 5 * 1024 * 1024)
                throw new ArgumentException(
                    "The image must not exceed 5 MB.");

            var allowedTypes = new Dictionary<string, string>
            {
                { "image/jpeg", ".jpg" },
                { "image/png", ".png" },
                { "image/gif", ".gif" },
                { "image/webp", ".webp" }
            };

            if (!allowedTypes.TryGetValue(
                file.ContentType.ToLowerInvariant(),
                out string? extension))
            {
                throw new ArgumentException(
                    "Only JPG, PNG, GIF and WEBP images are allowed.");
            }

            if (folder != "profile-pictures" &&
                folder != "event-banners")
            {
                throw new ArgumentException("Invalid image folder.");
            }

            await _containerClient.CreateIfNotExistsAsync(
                PublicAccessType.Blob);

            string fileName =
                $"{folder}/{Guid.NewGuid()}{extension}";

            var blobClient =
                _containerClient.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();

            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    }
                });

            return blobClient.Uri.ToString();
        }
    }
}
