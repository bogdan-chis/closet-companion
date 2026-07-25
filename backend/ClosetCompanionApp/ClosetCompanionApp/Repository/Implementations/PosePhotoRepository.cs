using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClosetCompanionApp.Repository.Implementations
{
    public class PosePhotoRepository : IPosePhotoRepository
    {
        private readonly AppDbContext _context;
        public PosePhotoRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<PosePhoto> AddAsync(PosePhoto photo)
        {
            await _context.PosePhoto.AddAsync(photo);
            await _context.SaveChangesAsync();

            return photo;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PosePhoto>> GetAllAsync()
        {
            return await _context.PosePhoto.ToListAsync();
        }

        public async Task<PosePhoto?> GetByIdAsync(Guid id)
        {
            return await _context.PosePhoto.FindAsync(id);
        }

        public Task<PosePhoto> GetDefaultAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PosePhoto photo)
        {
            throw new NotImplementedException();
        }
    }
}
