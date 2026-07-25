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
            using var stream = file.OpenReadStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return await UploadInternalAsync(ms.ToArray(), file.ContentType, Path.GetExtension(file.FileName), folder);
        }

        public async Task<string> UploadBytesAsync(byte[] data, string contentType, string folder)
        {
            var extension = contentType switch
            {
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => ".jpg"
            };
            return await UploadInternalAsync(data, contentType, extension, folder);
        }

        private async Task<string> UploadInternalAsync(byte[] data, string contentType, string extension, string folder)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var objectPath = $"{folder}/{uniqueFileName}";

            using var content = new ByteArrayContent(data);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            var requestUrl = $"/storage/v1/object/{_bucketName}/{objectPath}";
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Supabase upload failed ({(int)response.StatusCode}): {errorBody}");
            }

            return $"{_httpClient.BaseAddress}storage/v1/object/public/{_bucketName}/{objectPath}";
        }
    }
}
