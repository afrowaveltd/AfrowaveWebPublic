using Id.Models.CommunicationModels;

namespace Id.Services
{
    public interface IRoleService
    {
		Task<bool> AssignDefaultRolesToNewUserAsync(string applicationId, string userId);
		Task<ApiResponse<int>> CreateApplicationRole(CreateApplicationRoleModel role);

        Task<ApiResponse<List<ApplicationRole>>> CreateDefaultRoles(string applicationId);

        Task<ApiResponse<bool>> DeleteApplicationRole(int id);

        Task<ApiResponse<List<ApplicationRole>>> GetApplicationRoles(string applicationId);

        Task<ApiResponse<bool>> UpdateApplicationRole(int id, CreateApplicationRoleModel role);
    }
}