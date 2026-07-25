namespace ClosetCompanionApp.Domain
{
    public class GeneratedOutfit
    {
        public Guid Id { get; private set; }
        public Guid BasePhotoId { get; private set; }
        public List<Guid> SelectedGarmentIds { get; private set; }
        public string ResultImageUrl { get; private set; }
        public bool IsFavorite { get; private set; }
        public DateTime GeneratedOn { get; private set; }

        public GeneratedOutfit(Guid basePhotoId, List<Guid> selectedGarmentIds, string resultImageUrl)
        {
            Id = Guid.NewGuid();
            BasePhotoId = basePhotoId;
            SelectedGarmentIds = selectedGarmentIds ?? new List<Guid>();
            ResultImageUrl = resultImageUrl;
            IsFavorite = false;
            GeneratedOn = DateTime.UtcNow;
        }

        public void ToggleFavorite() => IsFavorite = !IsFavorite;
    }
}
