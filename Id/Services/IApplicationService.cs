using Id.Models.CommunicationModels;

namespace Id.Services
{
	public interface IApplicationService
	{
		Task<string> CheckApplicationId(string applicationId);

		string GetApplicationIconPath(string applicationId, LogoSize size);

		string GetApplicationIconPath(string applicationId);

		string GetApplicationImagePath(string applicationId);

		Task<string> GetDefaultApplicationId();

		Task<ApplicationPublicInfo?> GetPublicInfoAsync(string applicationId);

		Task<bool> IsApplicationNameUnique(string name);

		Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationModel input);
	}
}