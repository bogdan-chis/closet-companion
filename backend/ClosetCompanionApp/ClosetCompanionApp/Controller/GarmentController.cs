using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Service.Interfaces;
using ClosetCompanionApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class GarmentController : ControllerBase
    {
        private readonly IGarmentService _service;

        public GarmentController(IGarmentService service)
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
        public async Task<IActionResult> Add([FromBody] CreateGarmentDto dto)
        {
            try
            {
                await _service.AddAsync(dto.Name, dto.Category, dto.ImageUrl, dto.SourceWebsiteUrl ?? "");
                return Ok(new { message = "Garment added successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
