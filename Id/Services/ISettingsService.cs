using Id.Models.SettingsModels;

namespace Id.Services
{
	public interface ISettingsService
	{
		Task<CookieSettings> GetCookieSettingsAsync();
		Task<CorsSettings> GetCorsSettingsAsync();
		Task<JwtSettings> GetJwtSettingsAsync();
		Task<LoginRules> GetLoginRulesAsync();
		Task<PasswordRules> GetPasswordRulesAsync();
		Task<IdentificatorSettings> GetSettingsAsync();

		Task SetApplicationId(string id);

		Task SetSettingsAsync(IdentificatorSettings settings);
	}
}