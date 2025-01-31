using Id.Models.SettingsModels;

namespace Id.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class PasswordRequirements(ISettingsService settingsService) : ControllerBase
	{
		private readonly ISettingsService _settingsService = settingsService;
		private IdentificatorSettings _settings = new();

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			_settings = await _settingsService.GetSettingsAsync();
			return Ok(_settings.PasswordRules);
		}
	}
}