using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Repository
{
    public interface IGarmentRepository
    {
        Task<Garment> AddAsync(Garment garment);
        Task DeleteAsync(Guid id);
        Task<Garment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Garment>> GetAllAsync();
    }
}
