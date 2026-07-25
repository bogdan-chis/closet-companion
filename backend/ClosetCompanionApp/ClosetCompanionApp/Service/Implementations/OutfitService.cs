using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class OutfitService : IOutfitService
    {
        private readonly IOutfitRepository _repository;
        public OutfitService(IOutfitRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Guid posePhotoId, List<Guid> garmentIds, string resultImageUrl)
        {
            if (posePhotoId == Guid.Empty)
                throw new ArgumentException("Pose photo ID cannot be empty.", nameof(posePhotoId));

            if (garmentIds == null || garmentIds.Count == 0)
                throw new ArgumentException("At least one garment ID must be provided.", nameof(garmentIds));

            if (string.IsNullOrWhiteSpace(resultImageUrl))
                throw new ArgumentException("Result image URL cannot be empty.", nameof(resultImageUrl));

            var outfit = new GeneratedOutfit(posePhotoId, garmentIds, resultImageUrl);
            
            await _repository.AddAsync(outfit);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<GeneratedOutfit?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetFavouritesAsync()
        {
            return await _repository.GetFavoritesAsync();
        }
    }
}
