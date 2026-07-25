using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class GarmentService : IGarmentService
    {
        private readonly IGarmentRepository _repository;

        public GarmentService(IGarmentRepository repository)
        {
            _repository = repository;
        }
        
        public async Task AddAsync(string name, GarmentCategory category, string imageUrl, string sourceWebsiteUrl = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Garment name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

            var garment = new Garment(name, category, imageUrl, sourceWebsiteUrl);

            await _repository.AddAsync(garment);
        }

        public async Task DeleteAsync(Guid id)
        {
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
