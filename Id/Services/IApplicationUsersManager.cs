/* This file contains the ApplicationUsersManager class. This class is responsible for managing the application users. It contains functions for deleting, updating, and registering application users.
 * It also contains functions for getting the user description by user id and getting the user id by application user id.
 * The class is used by the ApplicationUsersController.
 */

using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Interface for managing application users.
	/// </summary>
	public interface IApplicationUsersManager
	{
		/// <summary>
		/// Deletes an application user by id.
		/// </summary>
		/// <param name="applicationUserId">Application user ID</param>
		/// <returns>Delete result with ID of the deleted applicationUser</returns>
		Task<DeleteResult<int>> DeleteApplicationUserAsync(int applicationUserId);

		/// <summary>
		/// Get the application user description by user id.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="applicationId">Application ID</param>
		/// <returns>string with ApplicationUserDescription</returns>
		Task<string> GetApplicationUserDescriptionByUserIdAsync(string userId, string applicationId);

		/// <summary>
		/// Get the user ID by application user ID.
		/// </summary>
		/// <param name="applicationUserId">Application User ID</param>
		/// <returns>User ID for authenticator app</returns>
		Task<string> GetUserIdByApplicationUserIdAsync(int applicationUserId);

		/// <summary>
		/// Register an application user.
		/// </summary>
		/// <param name="input">RegisterApplicationUserInput</param>
		/// <returns>RegisterApplicationUserResult</returns>
		Task<RegisterApplicationUserResult> RegisterApplicationUserAsync(RegisterApplicationUserInput input);

		/// <summary>
		/// Update an application user.
		/// </summary>
		/// <param name="input">UpdateApplicationUserInput</param>
		/// <returns>UpdateResult</returns>
		Task<UpdateResult> UpdateApplicationUserAsync(UpdateApplicationUserInput input);
	}
}