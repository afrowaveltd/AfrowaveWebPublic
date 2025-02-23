using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IApplicationsManager
	{
		Task<bool> ApplicationExistsAsync(string applicationId);

		Task<ApplicationSmtpSettings?> GetApplicationSmtpSettingsAsync(string applicationId);

		Task<string> GetAuthenticatorIdAsync();

		string GetFullsizeLogoPath(string applicationId);

		string GetIconPath(string applicationId);

		Task<ApplicationView?> GetInfoAsync(string applicationId);

		string GetLogoPath(string applicationId, LogoSize size);

		Task<bool> IsNameUnique(string name);

		Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationInput input);

		Task<RegisterSmtpResult> RegisterApplicationSmtpSettingsAsync(RegisterSmtpInput input);

		Task<UpdateResult> UpdateApplicationAsync(UpdateApplicationInput input);

		Task<UpdateResult> UpdateApplicationSmtpSettingsAsync(UpdateSmtpInput input);
	}
}