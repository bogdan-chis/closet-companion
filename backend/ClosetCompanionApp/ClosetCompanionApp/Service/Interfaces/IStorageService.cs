namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
        Task<string> UploadBytesAsync(byte[] data, string contentType, string folder); // NEW
    }
}