using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class PosePhotoService : IPosePhotoService
    {
        private readonly IPosePhotoRepository _repository;

        public PosePhotoService(IPosePhotoRepository repository)
        {
            _repository = repository;
        }

        public async Task<PosePhoto> AddAsync(string name, PoseCategory poseCategory, string imageUrl, bool isDefault = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Pose photo name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

            var photo = new PosePhoto(name, poseCategory, imageUrl, isDefault);

            return await _repository.AddAsync(photo);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PosePhoto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PosePhoto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
