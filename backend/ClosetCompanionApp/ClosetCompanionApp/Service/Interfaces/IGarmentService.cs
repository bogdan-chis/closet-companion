using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Service.Interfaces
{
    public interface IGarmentService
    {
        Task<IEnumerable<Garment>> GetAllAsync();

        // We pass the raw data here so the service can validate it 
        // before creating the actual Garment entity.
        Task AddAsync(string name, GarmentCategory category, string imageUrl, string sourceWebsiteUrl = "");
        Task<Garment> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
