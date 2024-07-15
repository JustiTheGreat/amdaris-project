namespace AmdarisProject.Infrastructure.Options
{
    public class BlobStorageSettings
    {
        public required string StorageAccount { get; set; }
        public required string AccessKey { get; set;}
        public required string ProfilePictureBlobContainerName { get; set; }

    }
}
