using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Service.Interfaces
{
    public record GarmentGenerationInput(string ImageUrl, GarmentCategory Category);

    public interface IAiGenerationService
    {
        Task<string> GenerateOutfitAsync(string poseImageUrl, IEnumerable<GarmentGenerationInput> garments);
    }
}