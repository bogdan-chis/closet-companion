namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
    }
}
