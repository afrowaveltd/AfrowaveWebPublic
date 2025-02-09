using Id.Models.CommunicationModels;

namespace Id.Services
{
	public interface IUserService
	{
		Task<RoleAssignResult> AsignUserToRoleAsync(string userId, int roleId);

		Task<List<RoleAssignResult>> AsignUserToRolesAsync(string userId, List<int> roleIds);

		Task<List<RoleAssignResult>> AssignAllRolesToOwner(string userId, string applicationId);

		Task<ApiResponse<int>> CreateApplicationUserAsync(CreateApplicationUserModel user);

		Task<UserRegistrationResult> CreateUserAsync(RegisterApplicationUserModel user);

		Task<ApiResponse<bool>> DeleteApplicationUserAsync(int id);

		Task<ApiResponse<List<ApplicationUser>>> GetApplicationUsersAsync(string applicationId);

		Task<ApiResponse<List<ApplicationRole>>> GetUserRolesAsync(string userId);

		Task<bool> IsEmailUnique(string email);

		Task<ApiResponse<bool>> RemoveUserFromRoleAsync(string userId, int roleId);

		Task<SendOtpResult> SendOtpAsync(string email);

		Task<ApiResponse<bool>> UpdateApplicationUserAsync(int id, CreateApplicationUserModel user);

		Task<ApiResponse<bool>> UpdateApplicationUserAsync(int id, RegisterApplicationUserModel user);
	}
}