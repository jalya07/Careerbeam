using Careerbeam.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Careerbeam.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IWebHostEnvironment env)
    {
        // Сохраняем файлы в wwwroot/uploads
        _basePath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        var folderPath = Path.Combine(_basePath, folder);
        Directory.CreateDirectory(folderPath); // создаёт папку, если её ещё нет

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var fullPath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Возвращаем относительный путь для хранения в БД
        return Path.Combine("uploads", folder, fileName).Replace("\\", "/");
    }

    public void DeleteFile(string filePath)
    {
        var fullPath = Path.Combine(_basePath, "..", filePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}