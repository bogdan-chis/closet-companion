namespace ClosetCompanionApp.Dto
{
    public class CreateOutfitDto
    {
        public Guid PosePhotoId { get; set; }
        public List<Guid> GarmentIds { get; set; } = new();
        public string ResultImageUrl { get; set; } = null!;
    }
}
