using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Repository.Interfaces
{
    public interface ISettingsRepository
    {
        Task<AppSetting?> GetSettingsAsync();
        Task UpdateSettingsAsync(AppSetting settings);
    }
}
