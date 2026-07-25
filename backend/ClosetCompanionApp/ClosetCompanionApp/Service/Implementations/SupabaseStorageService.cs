using ClosetCompanionApp.Service.Interfaces;

namespace ClosetCompanionApp.Service.Implementations
{
    public class SupabaseStorageService : IStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bucketName;

        public SupabaseStorageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            var supabaseUrl = configuration["Supabase:Url"]
                ?? throw new InvalidOperationException("Supabase:Url is not configured.");
            var serviceRoleKey = configuration["Supabase:ServiceRoleKey"]
                ?? throw new InvalidOperationException("Supabase:ServiceRoleKey is not configured.");
            _bucketName = configuration["Supabase:BucketName"] ??
                throw new InvalidOperationException("Supabase:BucketName is not configured.");

            _httpClient.BaseAddress = new Uri(supabaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", serviceRoleKey);
            _httpClient.DefaultRequestHeaders.Add("apikey", serviceRoleKey);
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var objectPath = $"{folder}/{uniqueFileName}";

            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            var requestUrl = $"/storage/v1/object/{_bucketName}/{objectPath}";

            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Supabase upload failed ({(int)response.StatusCode}): {errorBody}");
            }

            var publicUrl = $"{_httpClient.BaseAddress}storage/v1/object/public/{_bucketName}/{objectPath}";
            return publicUrl;
        }
    }
}
