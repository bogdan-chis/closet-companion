namespace ClosetCompanionApp.Service.Interfaces
{
    public interface ISettingsService
    {
        Task<int> GetCreditsAsync();
        Task<bool> DecrementCreditsAsync();
    }
}
