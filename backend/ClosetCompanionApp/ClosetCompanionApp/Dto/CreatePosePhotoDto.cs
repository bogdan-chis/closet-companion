using ClosetCompanionApp.Domain;

namespace ClosetCompanionApp.Dto
{
    public class CreatePosePhotoDto
    {
        public string Name { get; set; } = null!;
        public PoseCategory PoseCategory { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsDefault { get; set; } = false;
    }
}
