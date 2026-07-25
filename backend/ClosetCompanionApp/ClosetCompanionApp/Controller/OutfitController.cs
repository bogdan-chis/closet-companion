using ClosetCompanionApp.Service.Interfaces;
using ClosetCompanionApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class OutfitController : ControllerBase
    {
        private readonly IOutfitService _service;

        public OutfitController(IOutfitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var outfits = await _service.GetAllAsync();
            return Ok(outfits);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOutfitDto Dto)
        {
            try
            {
                await _service.AddAsync(Dto.PosePhotoId, Dto.GarmentIds, Dto.ResultImageUrl);
                return Ok(new { message = "Outfit saved successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var outfit = await _service.GetByIdAsync(id);
            if (outfit == null)
            {
                return NotFound(new { message = $"Outfit with ID {id} was not found." });
            }
            return Ok(outfit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var outfit = await _service.GetByIdAsync(id);
            if (outfit == null)
            {
                return NotFound(new { message = $"Outfit with ID {id} was not found." });
            }

            await _service.DeleteAsync(id);
            return Ok(new { message = $"Outfit with ID {id} was successfully deleted." });
        }
    }
}
