using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IOutfitService
    {
        Task<GeneratedOutfit> CreatePendingAsync(Guid posePhotoId, List<Guid> garmentIds);
        Task MarkProcessingAsync(Guid id);
        Task CompleteAsync(Guid id, string resultImageUrl);
        Task FailAsync(Guid id, string errorMessage);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<GeneratedOutfit>> GetAllAsync();
        Task<GeneratedOutfit?> GetByIdAsync(Guid id);
        Task<IEnumerable<GeneratedOutfit>> GetFavouritesAsync();
    }
}