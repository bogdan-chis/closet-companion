using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IGarmentService
    {
        Task<IEnumerable<Garment>> GetAllAsync();
        Task AddAsync(string name, GarmentCategory category, string imageUrl, string sourceWebsiteUrl = "");
        Task<Garment?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
