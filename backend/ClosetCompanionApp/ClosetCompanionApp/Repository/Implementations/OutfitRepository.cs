using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClosetCompanionApp.Repository.Implementations
{
    public class OutfitRepository : IOutfitRepository
    {
        AppDbContext _context;
        public OutfitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GeneratedOutfit outfit)
        {
            await _context.GeneratedOutfits.AddAsync(outfit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var outfit = await _context.GeneratedOutfits.FindAsync(id);
            if (outfit != null)
            {
                _context.GeneratedOutfits.Remove(outfit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetAllAsync()
        {
            return await _context.GeneratedOutfits.ToListAsync();
        }

        public async Task<GeneratedOutfit?> GetByIdAsync(Guid id)
        {
            return await _context.GeneratedOutfits.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetFavoritesAsync()
        {
            return await _context.GeneratedOutfits
                .Where(o => o.IsFavorite)
                .OrderByDescending(o => o.GeneratedOn)
                .ToListAsync();
        }

        public async Task UpdateAsync(GeneratedOutfit outfit)
        {
            _context.GeneratedOutfits.Update(outfit);
            await _context.SaveChangesAsync();
        }
    }
}
