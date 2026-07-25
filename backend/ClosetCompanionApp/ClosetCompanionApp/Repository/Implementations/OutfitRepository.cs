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

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneratedOutfit> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetFavoritesAsync()
        {
            return await _context.GeneratedOutfits
                .Where(o => o.IsFavorite)
                .OrderByDescending(o => o.GeneratedOn)
                .ToListAsync();
        }

        public Task UpdateAsync(GeneratedOutfit outfit)
        {
            throw new NotImplementedException();
        }
    }
}
