namespace ClosetCompanionApp.Domain
{
    public class OutfitGarment
    {
        public Guid GeneratedOutfitId { get; private set; }
        public Guid GarmentId { get; private set; }

        private OutfitGarment() { }

        public OutfitGarment(Guid generatedOutfitId, Guid garmentId)
        {
            GeneratedOutfitId = generatedOutfitId;
            GarmentId = garmentId;
        }
    }
}
