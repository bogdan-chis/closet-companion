using System.Net.Http.Json;
using System.Text.Json;
using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class ReplicateAiGenerationService : IAiGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly IStorageService _storageService;

        private const string ModelVersion = "0513734a452173b8173e907e3a59d19a36266e55b48528559432bd21c7d7e985";

        public ReplicateAiGenerationService(HttpClient httpClient, IConfiguration configuration, IStorageService storageService)
        {
            _httpClient = httpClient;
            _storageService = storageService;

            var apiToken = configuration["Replicate:ApiToken"]
                ?? throw new InvalidOperationException("Replicate:ApiToken is not configured.");

            _httpClient.BaseAddress = new Uri("https://api.replicate.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Token", apiToken);
        }

        public async Task<string> GenerateOutfitAsync(string poseImageUrl, IEnumerable<GarmentGenerationInput> garments)
        {
            var currentImageUrl = poseImageUrl;

            // Chain garments one at a time — each result becomes the base for the next.
            foreach (var garment in garments)
            {
                currentImageUrl = await ApplyGarmentAsync(currentImageUrl, garment);
            }

            // Replicate's output URL expires — re-host permanently on our own bucket.
            return await RehostImageAsync(currentImageUrl);
        }

        private async Task<string> ApplyGarmentAsync(string humanImageUrl, GarmentGenerationInput garment)
        {
            var category = garment.Category switch
            {
                GarmentCategory.Top => "upper_body",
                GarmentCategory.Bottom => "lower_body",
                GarmentCategory.Dress => "dresses",
                _ => throw new InvalidOperationException(
                    $"Category '{garment.Category}' is not supported for outfit generation.")
            };

            var requestBody = new
            {
                version = ModelVersion,
                input = new
                {
                    human_img = humanImageUrl,
                    garm_img = garment.ImageUrl,
                    category,
                    garment_des = "clothing item",
                    crop = false,
                    steps = 30
                }
            };

            var response = await _httpClient.PostAsJsonAsync("predictions", requestBody);
            response.EnsureSuccessStatusCode();

            var prediction = await response.Content.ReadFromJsonAsync<JsonElement>();
            var predictionId = prediction.GetProperty("id").GetString()!;

            return await PollUntilCompleteAsync(predictionId);
        }

        private async Task<string> PollUntilCompleteAsync(string predictionId)
        {
            const int maxAttempts = 60; // ~5 minutes at 5s intervals
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                await Task.Delay(5000);

                var response = await _httpClient.GetAsync($"predictions/{predictionId}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                var status = result.GetProperty("status").GetString();

                if (status == "succeeded")
                {
                    var output = result.GetProperty("output");
                    return output.ValueKind == JsonValueKind.Array
                        ? output[0].GetString()!
                        : output.GetString()!;
                }

                if (status == "failed" || status == "canceled")
                {
                    var error = result.TryGetProperty("error", out var errProp)
                        ? errProp.GetString()
                        : "unknown error";
                    throw new Exception($"Replicate generation failed: {error}");
                }
                // "starting" / "processing" — keep polling
            }

            throw new TimeoutException("Replicate generation timed out after 5 minutes.");
        }

        private async Task<string> RehostImageAsync(string temporaryUrl)
        {
            using var imageResponse = await _httpClient.GetAsync(temporaryUrl);
            imageResponse.EnsureSuccessStatusCode();

            var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();
            var contentType = imageResponse.Content.Headers.ContentType?.MediaType ?? "image/png";

            return await _storageService.UploadBytesAsync(imageBytes, contentType, "generated");
        }
    }
}