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
                var newGarment = await _service.AddAsync(dto.Name, dto.Category, dto.ImageUrl, dto.SourceWebsiteUrl ?? "");
                return Ok(newGarment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var garment = await _service.GetByIdAsync(id);
            if (garment == null)
            {
                return NotFound(new { message = $"Garment with ID {id} was not found." });
            }
            return Ok(garment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var garment = await _service.GetByIdAsync(id);
            if (garment == null)
            {
                return NotFound(new { message = $"Garment with ID {id} was not found." });
            }

            await _service.DeleteAsync(id);
            return Ok(new { message = $"Garment with ID {id} was successfully deleted." });

        }
    }
}
