using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClosetCompanionApp.Repository.Implementations
{
    public class OutfitRepository : IOutfitRepository
    {
        private readonly AppDbContext _context;
        public OutfitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GeneratedOutfit outfit)
        {
            await _context.GeneratedOutfits.AddAsync(outfit);

            foreach (var garmentId in outfit.SelectedGarmentIds)
            {
                await _context.OutfitGarments.AddAsync(new OutfitGarment(outfit.Id, garmentId));
            }

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

        public async Task DeleteByGarmentIdAsync(Guid garmentId)
        {
            var affectedOutfitIds = await _context.OutfitGarments
                .Where(og => og.GarmentId == garmentId)
                .Select(og => og.GeneratedOutfitId)
                .Distinct()
                .ToListAsync();

            if (affectedOutfitIds.Count == 0) return;

            var affectedOutfits = await _context.GeneratedOutfits
                .Where(o => affectedOutfitIds.Contains(o.Id))
                .ToListAsync();

            _context.GeneratedOutfits.RemoveRange(affectedOutfits);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetAllAsync()
        {
            var outfits = await _context.GeneratedOutfits.ToListAsync();
            await HydrateGarmentsAsync(outfits);
            return outfits;
        }

        public async Task<GeneratedOutfit?> GetByIdAsync(Guid id)
        {
            var outfit = await _context.GeneratedOutfits.FirstOrDefaultAsync(o => o.Id == id);
            if (outfit == null) return null;

            var garmentIds = await _context.OutfitGarments
                .Where(og => og.GeneratedOutfitId == id)
                .Select(og => og.GarmentId)
                .ToListAsync();

            outfit.HydrateGarmentIds(garmentIds);
            return outfit;
        }

        public async Task<IEnumerable<GeneratedOutfit>> GetFavoritesAsync()
        {
            var outfits = await _context.GeneratedOutfits
                .Where(o => o.IsFavorite)
                .OrderByDescending(o => o.GeneratedOn)
                .ToListAsync();

            await HydrateGarmentsAsync(outfits);
            return outfits;
        }

        public async Task UpdateAsync(GeneratedOutfit outfit)
        {
            _context.GeneratedOutfits.Update(outfit);
            await _context.SaveChangesAsync();
        }

        // Batch-loads join rows for a list of outfits in one query instead of
        // one query per outfit (avoids N+1).
        private async Task HydrateGarmentsAsync(List<GeneratedOutfit> outfits)
        {
            if (outfits.Count == 0) return;

            var outfitIds = outfits.Select(o => o.Id).ToList();
            var junctions = await _context.OutfitGarments
                .Where(og => outfitIds.Contains(og.GeneratedOutfitId))
                .ToListAsync();

            var grouped = junctions
                .GroupBy(og => og.GeneratedOutfitId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.GarmentId).ToList());

            foreach (var outfit in outfits)
            {
                outfit.HydrateGarmentIds(grouped.TryGetValue(outfit.Id, out var ids) ? ids : new List<Guid>());
            }
        }
    }
}