using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Repository
{
    public interface IPosePhotoRepository
    {
        Task AddAsync(PosePhoto photo);
        Task DeleteAsync(Guid id);
        Task<PosePhoto> GetByIdAsync(Guid id);
        Task<IEnumerable<PosePhoto>> GetAllAsync();
        Task<PosePhoto> GetDefaultAsync();
        Task UpdateAsync(PosePhoto photo);
    }
}
