using Microsoft.AspNetCore.Http;

namespace BookStore.BAL.Services;

public static class FileService
{
    public static async Task<string> Save(IFormFile file)
    {
        bool isImage = file.ContentType.StartsWith("image");
        string type = isImage ? "images/book" : "files";
        string filePath = Path.Combine($"/{type}/", file.FileName);
        using (var fileStream = new FileStream("wwwroot" + filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filePath;
    }
    public static void Delete(string sourceFile)
    {
        if (sourceFile.Contains("/server/")) return;
        if (File.Exists("wwwroot" + sourceFile))
            File.Delete("wwwroot" + sourceFile);
    }

    public static async Task<string> SaveProfilePic(IFormFile img)
    {
        string filePath = Path.Combine($"/images/profile/", img.FileName);
        using (var fileStream = new FileStream("wwwroot" + filePath, FileMode.Create))
        {
            await img.CopyToAsync(fileStream);
        }
        return filePath;
    }
}
