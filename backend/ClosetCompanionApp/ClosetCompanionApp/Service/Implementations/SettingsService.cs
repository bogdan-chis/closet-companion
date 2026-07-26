using ClosetCompanionApp.Repository.Interfaces;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<int> GetCreditsAsync()
        {
            var settings = await _settingsRepository.GetSettingsAsync();
            return settings?.Credits ?? 0;
        }

        public async Task<bool> DecrementCreditsAsync()
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            if (settings == null || settings.Credits <= 0)
            {
                return false;
            }

            settings.Credits -= 1;
            await _settingsRepository.UpdateSettingsAsync(settings);

            return true;
        }
    }
}
