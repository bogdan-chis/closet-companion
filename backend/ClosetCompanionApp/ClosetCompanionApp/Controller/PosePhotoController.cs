using ClosetCompanionApp.Domain;
using ClosetCompanionApp.Service.Interfaces;
using ClosetCompanionApp.Dto;
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
                await _service.AddAsync(Dto.Name, Dto.PoseCategory, Dto.ImageUrl, Dto.IsDefault);
                return Ok(new { message = "Pose photo added successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
