using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Repository
{
    public interface IOutfitRepository
    {
        Task AddAsync(GeneratedOutfit outfit);
        Task DeleteAsync(Guid id);
        Task<GeneratedOutfit> GetByIdAsync(Guid id);
        Task<IEnumerable<GeneratedOutfit>> GetFavoritesAsync();
        Task UpdateAsync(GeneratedOutfit outfit);
    }
}
