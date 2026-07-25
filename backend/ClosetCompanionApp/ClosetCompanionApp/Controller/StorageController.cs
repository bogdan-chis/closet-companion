using ClosetCompanionApp.Service.Interfaces;
using ClosetCompanionApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly ILogger<StorageController> _logger;

        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg", "image/png", "image/webp", "image/heic"
        };

        public StorageController(IStorageService storageService, ILogger<StorageController> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(MaxFileSizeBytes)]
        public async Task<ActionResult<UploadResponseDto>> Upload(IFormFile file, [FromForm] string folder = "misc")
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was provided.");

            if (file.Length > MaxFileSizeBytes)
                return BadRequest("File exceeds the 10MB limit.");

            if (!AllowedContentTypes.Contains(file.ContentType))
                return BadRequest($"Unsupported content type: {file.ContentType}");

            try
            {
                var imageUrl = await _storageService.UploadFileAsync(file, folder);

                return Ok(new UploadResponseDto
                {
                    ImageUrl = imageUrl,
                    FileName = file.FileName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName} to storage.", file.FileName);
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
    }
}
