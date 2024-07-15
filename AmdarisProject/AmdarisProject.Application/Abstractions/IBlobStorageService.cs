using Microsoft.AspNetCore.Http;

namespace AmdarisProject.Application.Abstractions
{
    public interface IBlobStorageService
    {
        Task<string> UploadFile(IFormFile file);

        Task<string> UpdateFile(string fileName, IFormFile file);

        Task DeleteFile(string fileName);
    }
}
