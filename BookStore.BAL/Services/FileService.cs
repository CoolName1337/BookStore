using Microsoft.AspNetCore.Http;

namespace BookStore.BAL.Services;

public static class FileService
{
    public static async Task<string> SaveBookFile(IFormFile file, string name)
    {
        string fileName = string.Join(".", name.Replace(" ", ""), file.FileName.Split(".").Last());
        bool isImage = file.ContentType.StartsWith("image");
        string type = isImage ? "images/book" : "files";
        string filePath = Path.Combine($"/{type}/", fileName);
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

    public static async Task<string> SaveProfilePic(IFormFile img, string imgName)
    {
        string fileName = string.Join(".", imgName, img.FileName.Split("/").Last());
        string filePath = Path.Combine($"/images/profile/", fileName);
        using (var fileStream = new FileStream("wwwroot" + filePath, FileMode.Create))
        {
            await img.CopyToAsync(fileStream);
        }
        return filePath;
    }
    public static async Task<string> SaveAuthorPic(IFormFile img, string imgName)
    {
        string fileName = string.Join(".", imgName, img.FileName.Split("/").Last());
        string filePath = Path.Combine($"/images/author/", fileName);
        using (var fileStream = new FileStream("wwwroot" + filePath, FileMode.Create))
        {
            await img.CopyToAsync(fileStream);
        }
        return filePath;
    }
}
