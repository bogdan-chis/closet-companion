using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository;
using ClosetCompanionApp.Repository.Interfaces;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class GarmentService : IGarmentService
    {
        private readonly IGarmentRepository _repository;
        private readonly IOutfitRepository _outfitRepository;

        public GarmentService(IGarmentRepository repository, IOutfitRepository outfitRepository)
        {
            _repository = repository;
            _outfitRepository = outfitRepository;
        }

        public async Task<Garment> AddAsync(string name, GarmentCategory category, string imageUrl, string sourceWebsiteUrl = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Garment name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

            var garment = new Garment(name, category, imageUrl, sourceWebsiteUrl);

            return await _repository.AddAsync(garment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _outfitRepository.DeleteByGarmentIdAsync(id);
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Garment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<Garment?> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}