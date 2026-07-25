using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class OutfitGenerationBackgroundService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutfitGenerationBackgroundService> _logger;

        public OutfitGenerationBackgroundService(
            IBackgroundTaskQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<OutfitGenerationBackgroundService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var outfitId in _queue.Reader.ReadAllAsync(stoppingToken))
            {
                await ProcessAsync(outfitId);
            }
        }

        private async Task ProcessAsync(Guid outfitId)
        {
            using var scope = _scopeFactory.CreateScope();
            var outfitService = scope.ServiceProvider.GetRequiredService<IOutfitService>();
            var garmentService = scope.ServiceProvider.GetRequiredService<IGarmentService>();
            var poseService = scope.ServiceProvider.GetRequiredService<IPosePhotoService>();
            var aiService = scope.ServiceProvider.GetRequiredService<IAiGenerationService>();

            try
            {
                var outfit = await outfitService.GetByIdAsync(outfitId);
                if (outfit == null) return;

                await outfitService.MarkProcessingAsync(outfitId);

                var pose = await poseService.GetByIdAsync(outfit.BasePhotoId);
                if (pose == null) throw new Exception("Pose photo not found.");

                var garmentInputs = new List<GarmentGenerationInput>();
                foreach (var garmentId in outfit.SelectedGarmentIds)
                {
                    var garment = await garmentService.GetByIdAsync(garmentId);
                    if (garment == null) continue;

                    // VTON models aren't trained for footwear
                    if (garment.Category == GarmentCategory.Shoes) continue;

                    garmentInputs.Add(new GarmentGenerationInput(garment.ImageUrl, garment.Category));
                }

                var resultUrl = await aiService.GenerateOutfitAsync(pose.ImageUrl, garmentInputs);
                await outfitService.CompleteAsync(outfitId, resultUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outfit generation failed for {OutfitId}", outfitId);
                await outfitService.FailAsync(outfitId, ex.Message);
            }
        }
    }
}