namespace Id.Services
{
	/// <summary>
	/// Service to handle settings.
	/// </summary>
	public interface ISettingsService
	{
		/// <summary>
		/// Get the application ID.
		/// </summary>
		/// <returns>Authenticator ApplicationId</returns>
		Task<string> GetApplicationIdAsync();

		/// <summary>
		/// Get the cookie settings.
		/// </summary>
		/// <returns>CookieSettings</returns>
		Task<CookieSettings> GetCookieSettingsAsync();

		/// <summary>
		/// Get the CORS settings.
		/// </summary>
		/// <returns>CorsSettings</returns>
		Task<CorsSettings> GetCorsSettingsAsync();

		/// <summary>
		/// Get the JWT settings.
		/// </summary>
		/// <returns>JwtSettings</returns>
		Task<JwtSettings> GetJwtSettingsAsync();

		/// <summary>
		/// Get the login rules.
		/// </summary>
		/// <returns>LoginRules</returns>
		Task<LoginRules> GetLoginRulesAsync();

		/// <summary>
		/// Get the password rules.
		/// </summary>
		/// <returns>PasswordRules</returns>
		Task<PasswordRules> GetPasswordRulesAsync();

		/// <summary>
		/// Get the settings.
		/// </summary>
		/// <returns>IdentificatorSettings</returns>
		Task<IdentificatorSettings> GetSettingsAsync();

		/// <summary>
		/// Set the application ID.
		/// </summary>
		/// <param name="id">Authenticator application ID</param>
		/// <returns></returns>
		Task SetApplicationId(string id);

		/// <summary>
		/// Update application settings.
		/// </summary>
		/// <param name="settings">IdentificatorSettings</param>
		/// <returns></returns>
		Task SetSettingsAsync(IdentificatorSettings settings);

		/// <summary>
		/// Determines whether themes are enabled in the application.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if themes are
		/// enabled; otherwise, <see langword="false"/>.</returns>
		Task<bool> ThemesEnabled();
	}
}