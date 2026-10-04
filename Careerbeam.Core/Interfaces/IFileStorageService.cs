using Microsoft.AspNetCore.Http;

namespace Careerbeam.Core.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string folder);
    void DeleteFile(string filePath);
}