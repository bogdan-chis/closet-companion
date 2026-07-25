using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Dto
{
    public class CreateGarmentDto
    {
        public string Name { get; set; } = null!;
        public GarmentCategory Category { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string SourceWebsiteUrl { get; set; } = null!;
    }
}
