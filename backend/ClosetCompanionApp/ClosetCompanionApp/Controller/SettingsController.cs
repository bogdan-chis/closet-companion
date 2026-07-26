using ClosetCompanionApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClosetCompanionApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("credits")]
        public async Task<IActionResult> GetCredits()
        {
            var credits = await _settingsService.GetCreditsAsync();
            return Ok(new { credits = credits });
        }
    }
}
