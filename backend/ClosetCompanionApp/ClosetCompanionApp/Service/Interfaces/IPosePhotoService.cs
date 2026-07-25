using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IPosePhotoService
    {
        Task<IEnumerable<PosePhoto>> GetAllAsync();
        Task<PosePhoto> AddAsync(string name, PoseCategory poseCategory, string imageUrl, bool isDefault = false);

        Task<PosePhoto?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
