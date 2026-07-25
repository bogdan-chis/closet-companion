using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClosetCompanionApp.Repository.Implementations
{
    public class GarmentRepository : IGarmentRepository
    {
        private readonly AppDbContext _context;
        public GarmentRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task AddAsync(Garment garment)
        {
            await _context.Garments.AddAsync(garment);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Garment>> GetAllAsync()
        {
            return await _context.Garments.ToListAsync();
        }

        public async Task<Garment?> GetByIdAsync(Guid id)
        {
            return await _context.Garments.FindAsync(id);
        }
    }
}
