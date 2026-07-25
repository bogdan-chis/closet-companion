using ClosetCompanionApp.Dto;
using ClosetCompanionApp.Service.Implementations;
using ClosetCompanionApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class OutfitController : ControllerBase
    {
        private readonly IOutfitService _service;
        private readonly IBackgroundTaskQueue _queue;

        public OutfitController(IOutfitService outfitService, IBackgroundTaskQueue queue)
        {
            _service = outfitService;
            _queue = queue;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateOutfitDto dto)
        {
            try
            {
                var outfit = await _service.CreatePendingAsync(dto.PoseId, dto.GarmentIds);
                _queue.QueueGeneration(outfit.Id);
                return Accepted(new { id = outfit.Id, status = outfit.Status.ToString() });
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
            return outfit == null ? NotFound() : Ok(outfit);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites() => Ok(await _service.GetFavouritesAsync());

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var outfit = await _service.GetByIdAsync(id);
            if (outfit == null)
            {
                return NotFound(new { message = $"Outfit with ID {id} was not found." });
            }

            await _service.DeleteAsync(id);
            return Ok(new { message = $"Outfit with ID {id} was successfully deleted." });
        }

        [HttpPost("{id}/favorite")]
        public async Task<IActionResult> ToggleFavorite(Guid id)
        {
            await _service.ToggleFavoriteAsync(id);
            var outfit = await _service.GetByIdAsync(id);
            return outfit == null ? NotFound() : Ok(outfit);
        }
    }
}
