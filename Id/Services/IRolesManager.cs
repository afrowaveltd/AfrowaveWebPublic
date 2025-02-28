using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Provides methods to manage roles.
	/// </summary>
	public interface IRolesManager
	{
		/// <summary>
		/// Creates a new application role.
		/// </summary>
		/// <param name="input">CreateRoleInput</param>
		/// <returns>CreateRoleResult</returns>
		Task<CreateRoleResult> CreateApplicationRoleAsync(CreateRoleInput input);

		/// <summary>
		/// Deletes an application role.
		/// </summary>
		/// <param name="roleId">Role ID</param>
		/// <returns>int ID of the deleted role</returns>
		Task<DeleteResult<int>> DeleteApplicationRoleAsync(int roleId);

		/// <summary>
		/// Gets all application roles.
		/// </summary>
		/// <param name="applicationId">Application ID</param>
		/// <returns>List of application roles</returns>
		Task<List<ApplicationRole>> GetAllApplicationRolesAsync(string applicationId);

		/// <summary>
		/// Gets roles assigned to the application user in the application.
		/// </summary>
		/// <param name="applicationUserId">Application user ID</param>
		/// <returns>List of roles</returns>
		Task<List<RoleAssignResult>> GetApplicationUserRolesAsync(int applicationUserId);

		/// <summary>
		/// Removes a role from the application user.
		/// </summary>
		/// <param name="applicationUserId">Application user ID</param>
		/// <param name="roleName">Name of the role</param>
		/// <returns>True if role was removed successfuly</returns>
		Task<bool> RemoveApplicationUserFromRoleAsync(int applicationUserId, string roleName);

		/// <summary>
		/// Sets all roles to the owner of the application.
		/// </summary>
		/// <param name="applicationId">Application Id</param>
		/// <param name="userId">User Id</param>
		/// <returns>List of RoleAssignResults</returns>
		Task<List<RoleAssignResult>> SetAllRolesToOwner(string applicationId, string userId);

		/// <summary>
		/// Sets the default roles to the application user.
		/// </summary>
		/// <param name="applicationUserId">Application user ID</param>
		/// <returns>List of the RoleAssignResult</returns>
		Task<List<RoleAssignResult>> SetApplicationUserDefaultRolesAsync(int applicationUserId);

		/// <summary>
		/// Sets the role to the application user.
		/// </summary>
		/// <param name="applicationUserId">Application user ID</param>
		/// <param name="roleId">Role ID</param>
		/// <returns>Role assign result</returns>
		Task<RoleAssignResult> SetApplicationUserRoleAsync(int applicationUserId, int roleId);

		/// <summary>
		/// Sets the role to the application user.
		/// </summary>
		/// <param name="applicationUserId">Application user ID</param>
		/// <param name="roleName">Name of the role</param>
		/// <returns></returns>
		Task<RoleAssignResult> SetApplicationUserRoleByNameAsync(int applicationUserId, string roleName);

		/// <summary>
		/// Sets the role to the user.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="roleId">Role ID</param>
		/// <returns>RoleAssignResult</returns>
		Task<RoleAssignResult> SetUserRoleAsync(string userId, int roleId);

		/// <summary>
		/// Sets the role to the user.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="applicationId">Application ID</param>
		/// <param name="rolename">Name of the role</param>
		/// <returns>RoleAssignResult</returns>
		Task<RoleAssignResult> SetUserRoleByNameAsync(string userId, string applicationId, string rolename);

		/// <summary>
		/// Updates the application role.
		/// </summary>
		/// <param name="input">UpdateRoleInput</param>
		/// <returns>UpdateResult</returns>
		Task<UpdateResult> UpdateApplicationRoleAsync(UpdateRoleInput input);
	}
}