/* This file contains the ApplicationUsersManager class. This class is responsible for managing the application users. It contains functions for deleting, updating, and registering application users.
 * It also contains functions for getting the user description by user id and getting the user id by application user id.
 * The class is used by the ApplicationUsersController.
 */

using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IApplicationUsersManager
	{
		Task<DeleteResult<int>> DeleteApplicationUserAsync(int applicationUserId);
		Task<string> GetApplicationUserDescriptionByUserIdAsync(string userId, string applicationId);
		Task<string> GetUserIdByApplicationUserIdAsync(int applicationUserId);
		Task<RegisterApplicationUserResult> RegisterApplicationUserAsync(RegisterApplicationUserInput input);
		Task<UpdateResult> UpdateApplicationUserAsync(UpdateApplicationUserInput input);
	}
}