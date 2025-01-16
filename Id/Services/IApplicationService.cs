using Id.Models.CommunicationModels;

namespace Id.Services
{
   public interface IApplicationService
   {
		string GetApplicationIconPath(string applicationId, LogoSize size);
		string GetApplicationIconPath(string applicationId);
		string GetApplicationImagePath(string applicationId);
		Task<bool> IsApplicationNameUnique(string name);
        Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationModel input);
    }
}