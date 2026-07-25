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

        public async Task<GeneratedOutfit> CreatePendingAsync(Guid posePhotoId, List<Guid> garmentIds)
        {
            if (posePhotoId == Guid.Empty)
                throw new ArgumentException("Pose photo ID cannot be empty.", nameof(posePhotoId));
            if (garmentIds == null || garmentIds.Count == 0)
                throw new ArgumentException("At least one garment ID must be provided.", nameof(garmentIds));

            var outfit = new GeneratedOutfit(posePhotoId, garmentIds);
            await _repository.AddAsync(outfit);
            return outfit;
        }

        public async Task MarkProcessingAsync(Guid id)
        {
            var outfit = await _repository.GetByIdAsync(id);
            if (outfit == null) return;
            outfit.MarkProcessing();
            await _repository.UpdateAsync(outfit);
        }

        public async Task CompleteAsync(Guid id, string resultImageUrl)
        {
            var outfit = await _repository.GetByIdAsync(id);
            if (outfit == null) return;
            outfit.Complete(resultImageUrl);
            await _repository.UpdateAsync(outfit);
        }

        public async Task FailAsync(Guid id, string errorMessage)
        {
            var outfit = await _repository.GetByIdAsync(id);
            if (outfit == null) return;
            outfit.Fail(errorMessage);
            await _repository.UpdateAsync(outfit);
        }

        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);

        public async Task<IEnumerable<GeneratedOutfit>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<GeneratedOutfit?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<GeneratedOutfit>> GetFavouritesAsync() => await _repository.GetFavoritesAsync();
    }
}