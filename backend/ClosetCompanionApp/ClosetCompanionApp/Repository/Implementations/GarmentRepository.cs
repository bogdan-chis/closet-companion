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
        public async Task<Garment> AddAsync(Garment garment)
        {
            await _context.Garments.AddAsync(garment);
            await _context.SaveChangesAsync();

            return garment;
        }

        public async Task DeleteAsync(Guid id)
        {
            var associatedOutfits = await _context.GeneratedOutfits
                .Where(o => o.SelectedGarmentIds.Contains(id))
                .ToListAsync();

            if (associatedOutfits.Any())
            {
                _context.GeneratedOutfits.RemoveRange(associatedOutfits);
            }

            var garment = await _context.Garments.FindAsync(id);
            if (garment != null)
            {
                _context.Garments.Remove(garment);
            }

            await _context.SaveChangesAsync();
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
