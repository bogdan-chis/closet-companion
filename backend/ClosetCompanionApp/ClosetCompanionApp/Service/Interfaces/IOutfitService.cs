using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IOutfitService
    {
        Task AddAsync(Guid posePhotoId, List<Guid> garmentIds, string resultImageUrl);
        Task<IEnumerable<GeneratedOutfit>> GetAllAsync();
        Task<IEnumerable<GeneratedOutfit>> GetFavouritesAsync();
        Task<GeneratedOutfit> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
