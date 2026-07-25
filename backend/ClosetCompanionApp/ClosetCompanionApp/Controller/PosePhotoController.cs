using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Dto;
using ClosetCompanionApp.Service.Implementations;
using ClosetCompanionApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PosePhotoController : ControllerBase
    {
        private readonly IPosePhotoService _service;

        public PosePhotoController(IPosePhotoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var photos = await _service.GetAllAsync();
            return Ok(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePosePhotoDto Dto)
        {
            try
            {
                var photo = await _service.AddAsync(Dto.Name, Dto.PoseCategory, Dto.ImageUrl, Dto.IsDefault);
                return Ok(photo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var outfit = await _service.GetByIdAsync(id);
            if (outfit == null)
            {
                return NotFound(new { message = $"BasePhoto with ID {id} was not found." });
            }

            await _service.DeleteAsync(id);
            return Ok(new { message = $"BasePhoto with ID {id} was successfully deleted." });
        }
    }
}
