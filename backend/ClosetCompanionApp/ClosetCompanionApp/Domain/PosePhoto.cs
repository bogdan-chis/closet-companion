namespace ClosetCompanionApp.Domain
{
    public class PosePhoto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public PoseCategory PoseType { get; private set; }
        public string ImageUrl {  get; private set; }
        public bool IsDefault { get; private set; }
        public DateTime AddedOn { get; private set; }

        public PosePhoto(Guid id, string name, PoseCategory poseType, string imageUrl, bool isDefault, DateTime addedOn)
        {
            Id = id;
            Name = name;
            PoseType = poseType;
            ImageUrl = imageUrl;
            IsDefault = isDefault;
            AddedOn = addedOn;
        }

        public void SetAsDefault() => IsDefault = true;
        public void RemoveDefault() => IsDefault = false;
        public void UpdateName(string name) => Name = name;
    }
}
