using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class GarmentsController : ControllerBase
    {
        private readonly IGarmentService _service;

        public GarmentsController(IGarmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var garments = await _service.GetAllAsync();
            return Ok(garments);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateGarmentRequest request)
        {
            try
            {
                await _service.AddAsync(request.Name, request.Category, request.ImageUrl, request.SourceWebsiteUrl ?? "");
                return Ok(new { message = "Garment added successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    // This is a simple Data Transfer Object (DTO) to define what 
    // JSON data we expect the client to send us in the POST request.
    public class CreateGarmentRequest
    {
        public string Name { get; set; } = null!;
        public GarmentCategory Category { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? SourceWebsiteUrl { get; set; }
    }
}
