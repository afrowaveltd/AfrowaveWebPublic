using Id.Models.CommunicationModels;

namespace Id.Services
{
   public interface IApplicationService
   {
      Task<bool> IsApplicationNameUnique(string name);
        Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationModel input);
    }
}