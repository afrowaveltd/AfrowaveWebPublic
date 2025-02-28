using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Interface for managing applications.
	/// </summary>
	public interface IApplicationsManager
	{
		/// <summary>
		/// Check if an application exists.
		/// </summary>
		/// <param name="applicationId">Application ID</param>
		/// <returns>true if application exists, otherwise false</returns>
		Task<bool> ApplicationExistsAsync(string applicationId);

		/// <summary>
		/// Get the application SMTP settings.
		/// </summary>
		/// <param name="applicationId">Application ID</param>
		/// <returns>Gets Application SMTP settings</returns>
		Task<ApplicationSmtpSettings?> GetApplicationSmtpSettingsAsync(string applicationId);

		/// <summary>
		/// Get the application user ID.
		/// </summary>
		/// <returns>string with Authenticator application ID</returns>
		Task<string> GetAuthenticatorIdAsync();

		/// <summary>
		/// Get the full size logo path.
		/// </summary>
		/// <param name="applicationId"></param>
		/// <returns>string with the relative path to the logo</returns>
		string GetFullsizeLogoPath(string applicationId);

		/// <summary>
		/// Get the icon path.
		/// </summary>
		/// <param name="applicationId">Application ID</param>
		/// <returns>string with the relative path to the icon</returns>
		string GetIconPath(string applicationId);

		/// <summary>
		/// Get the application information.
		/// </summary>
		/// <param name="applicationId">Application ID</param>
		/// <returns>ApplicationView class</returns>
		Task<ApplicationView?> GetInfoAsync(string applicationId);

		/// <summary>
		/// Get the logo path.
		/// </summary>
		/// <param name="applicationId">Application ID</param>
		/// <param name="size">Size from enum</param>
		/// <returns>string with the relative path to the application logo</returns>
		string GetLogoPath(string applicationId, LogoSize size);

		/// <summary>
		/// Checks if the application name is unique.
		/// </summary>
		/// <param name="name">Application name</param>
		/// <returns>True if the name is not yet registered</returns>
		Task<bool> IsNameUnique(string name);

		/// <summary>
		/// Register an application.
		/// </summary>
		/// <param name="input">RegisterApplicationInput</param>
		/// <returns>RegisterApplicationResult class</returns>
		Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationInput input);

		/// <summary>
		/// Register the application SMTP settings.
		/// </summary>
		/// <param name="input">RegisterSmtpInput</param>
		/// <returns>RegisterSmtpResult</returns>
		Task<RegisterSmtpResult> RegisterApplicationSmtpSettingsAsync(RegisterSmtpInput input);

		/// <summary>
		/// Update the application.
		/// </summary>
		/// <param name="input">UpdateApplicationInput</param>
		/// <returns>the UpdateResult</returns>
		Task<UpdateResult> UpdateApplicationAsync(UpdateApplicationInput input);

		/// <summary>
		/// Update the application SMTP settings.
		/// </summary>
		/// <param name="input">UpdateSmtpInput</param>
		/// <returns>the UpdateResult</returns>
		Task<UpdateResult> UpdateApplicationSmtpSettingsAsync(UpdateSmtpInput input);
	}
}