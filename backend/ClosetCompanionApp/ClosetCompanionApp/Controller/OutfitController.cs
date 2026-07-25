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
    }
}
