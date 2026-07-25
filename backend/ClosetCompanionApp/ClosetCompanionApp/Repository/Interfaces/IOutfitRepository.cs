using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Repository.Interfaces
{
    public interface IOutfitRepository
    {
        Task AddAsync(GeneratedOutfit outfit);
        Task DeleteAsync(Guid id);
        Task<GeneratedOutfit> GetByIdAsync(Guid id);
        Task<IEnumerable<GeneratedOutfit>> GetAllAsync();
        Task<IEnumerable<GeneratedOutfit>> GetFavoritesAsync();
        Task UpdateAsync(GeneratedOutfit outfit);
    }
}
