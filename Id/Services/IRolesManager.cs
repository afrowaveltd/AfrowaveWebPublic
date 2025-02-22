using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IRolesManager
	{
		Task<CreateRoleResult> CreateApplicationRoleAsync(CreateRoleInput input);
		Task<DeleteResult<int>> DeleteApplicationRoleAsync(int roleId);
		Task<List<ApplicationRole>> GetAllApplicationRolesAsync(string applicationId);
		Task<List<RoleAssignResult>> GetApplicationUserRolesAsync(int applicationUserId);
		Task<bool> RemoveApplicationUserFromRoleAsync(int applicationUserId, string roleName);
		Task<List<RoleAssignResult>> SetAllRolesToOwner(string applicationId, string userId);
		Task<List<RoleAssignResult>> SetApplicationUserDefaultRolesAsync(int applicationUserId);
		Task<RoleAssignResult> SetApplicationUserRoleAsync(int applicationUserId, int roleId);
		Task<RoleAssignResult> SetApplicationUserRoleByNameAsync(int applicationUserId, string roleName);
		Task<RoleAssignResult> SetUserRoleAsync(string userId, int roleId);
		Task<RoleAssignResult> SetUserRoleByNameAsync(string userId, string applicationId, string rolename);
		Task<UpdateResult> UpdateApplicationRoleAsync(UpdateRoleInput input);
	}
}