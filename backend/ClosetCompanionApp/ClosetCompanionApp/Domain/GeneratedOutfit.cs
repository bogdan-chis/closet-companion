namespace ClosetCompanionApp.Domain
{
    public class GeneratedOutfit
    {
        public Guid Id { get; private set; }
        public Guid BasePhotoId { get; private set; }
        public List<Guid> SelectedGarmentIds { get; private set; }
        public string? ResultImageUrl { get; private set; }
        public GenerationStatus Status { get; private set; }
        public string? ErrorMessage { get; private set; }
        public bool IsFavorite { get; private set; }
        public DateTime GeneratedOn { get; private set; }

        public GeneratedOutfit(Guid basePhotoId, List<Guid> selectedGarmentIds)
        {
            Id = Guid.NewGuid();
            BasePhotoId = basePhotoId;
            SelectedGarmentIds = selectedGarmentIds ?? new List<Guid>();
            ResultImageUrl = null;
            Status = GenerationStatus.Pending;
            IsFavorite = false;
            GeneratedOn = DateTime.UtcNow;
        }

        public void MarkProcessing() => Status = GenerationStatus.Processing;

        public void Complete(string resultImageUrl)
        {
            ResultImageUrl = resultImageUrl;
            Status = GenerationStatus.Completed;
            ErrorMessage = null;
        }

        public void Fail(string errorMessage)
        {
            Status = GenerationStatus.Failed;
            ErrorMessage = errorMessage;
        }

        public void ToggleFavorite() => IsFavorite = !IsFavorite;
    }
}