using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Repository.Interfaces
{
    public interface IPosePhotoRepository
    {
        Task<PosePhoto> AddAsync(PosePhoto photo);
        Task DeleteAsync(Guid id);
        Task<PosePhoto?> GetByIdAsync(Guid id);
        Task<IEnumerable<PosePhoto>> GetAllAsync();
        Task<PosePhoto> GetDefaultAsync();
        Task UpdateAsync(PosePhoto photo);
    }
}
