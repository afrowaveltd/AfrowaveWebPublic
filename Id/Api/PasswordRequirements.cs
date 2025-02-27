using Id.Models.SettingsModels;

namespace Id.Api
{
	/// <summary>
	/// API controller for retrieving password requirements.
	/// </summary>
	/// <param name="settingsService">The settings service</param>
	[Route("api/[controller]")]
	[ApiController]
	public class PasswordRequirements(ISettingsService settingsService) : ControllerBase
	{
		private readonly ISettingsService _settingsService = settingsService;
		private IdentificatorSettings _settings = new();

		/// <summary>
		/// Retrieves the password requirements.
		/// </summary>
		/// <returns>PasswordRules</returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			_settings = await _settingsService.GetSettingsAsync();
			return Ok(_settings.PasswordRules);
		}
	}
}