using Microsoft.AspNetCore.Http;

namespace BookStore.BAL.Services;

public static class FileService
{
    public static async Task<string> Save(IFormFile file)
    {
        if(file == null)
        {
            return null;
        }
        bool isImage = file.ContentType.StartsWith("image");
        string type = isImage ? "images" : "files";
        string filePath = Path.Combine($"/{type}/", file.FileName);
        using (var fileStream = new FileStream("wwwroot" + filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filePath;
    }
    public static void Delete(string sourceFile)
    {
        if (File.Exists("wwwroot" + sourceFile))
            File.Delete("wwwroot" + sourceFile);
    }
}
