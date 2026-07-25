namespace ClosetCompanionApp.Dto
{
    public class GenerateOutfitDto
    {
        public Guid PoseId { get; set; }
        public List<Guid> GarmentIds { get; set; } = new();
    }
}