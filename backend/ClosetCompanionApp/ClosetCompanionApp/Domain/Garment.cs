namespace ClosetCompanionApp.Domain
{
    public class Garment
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public GarmentCategory Category { get; private set; }
        public string ImageUrl { get; private set; }
        public string? SourceWebsiteUrl { get; private set; }
        public DateTime AddedOn { get; private set; }

        public Garment(Guid id, string name, GarmentCategory category, string imageUrl, string? sourceWebsiteUrl, DateTime addedOn)
        {
            Id = id;
            Name = name;
            Category = category;
            ImageUrl = imageUrl;
            SourceWebsiteUrl = sourceWebsiteUrl;
            AddedOn = addedOn;
        }
    }
}
