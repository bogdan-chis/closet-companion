using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Repository.Interfaces;

namespace ClosetCompanionApp.Repository.Implementations
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDbContext _context;

        public SettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppSetting?> GetSettingsAsync()
        {
            return await _context.AppSettings.FindAsync(1);
        }

        public async Task UpdateSettingsAsync(AppSetting settings)
        {
            _context.AppSettings.Update(settings);
            await _context.SaveChangesAsync();
        }
    }
}
