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
	/// <param name="t"></param>
	/// <param name="context"></param>
	public class ApplicationUsersManager(IStringLocalizer<ApplicationUsersManager> t,
		ApplicationDbContext context) : IApplicationUsersManager
	{
		// Initialization

		private readonly ApplicationDbContext _context = context;
		private readonly IStringLocalizer<ApplicationUsersManager> _t = t;

		// Public functions

		/// <summary>
		/// Deletes an application user by id.
		/// </summary>
		/// <param name="applicationUserId">The applicationUserId of user to be deleted</param>
		/// <returns>DeleteResult with Id of deleted user</returns>
		/// <example>
		/// Request:
		/// await DeleteApplicationUserAsync(1);
		/// Response:
		/// {
		///		Success: true,
		///		DeletedId: 1,
		///		ErrorMessage: null
		///	 }
		///	 </example>
		public async Task<DeleteResult<int>> DeleteApplicationUserAsync(int applicationUserId)
		{
			DeleteResult<int> result = new();
			if(applicationUserId == 0)
			{
				result.Success = false;
				result.ErrorMessage = _t["Id is 0"];
				return result;
			}
			ApplicationUser? user = await _context.ApplicationUsers.FindAsync(applicationUserId);
			if(user == null)
			{
				result.Success = false;
				result.ErrorMessage = _t["User not found"];
				return result;
			}
			result.DeletedId = applicationUserId;
			_ = _context.ApplicationUsers.Remove(user);
			_ = await _context.SaveChangesAsync();
			result.Success = true;
			return result;
		}

		/// <summary> Gets the user description by user id. </summary>
		/// <param name="applicationId">The applicationId of the user</param>
		/// <param name="userId">The userId of the user</param>
		/// <returns>The user description</returns>
		/// <example>
		/// Request:
		/// await GetApplicationUserDescriptionByUserIdAsync("123", "456");
		/// Response: "User description"
		/// </example>
		public async Task<string> GetApplicationUserDescriptionByUserIdAsync(string userId, string applicationId)
		{
			ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.ApplicationId == applicationId);
			return user?.UserDescription ?? string.Empty;
		}

		/// <summary> Gets the user id by application user id. </summary>
		/// <param name="applicationUserId">The applicationUserId of the user</param>
		/// <returns>The userId of the user</returns>
		/// <example>
		/// Request:
		/// await GetUserIdByApplicationUserIdAsync(1);
		/// Response: "123"
		/// </example>
		public async Task<string> GetUserIdByApplicationUserIdAsync(int applicationUserId)
		{
			ApplicationUser? user = await _context.ApplicationUsers.FindAsync(applicationUserId);
			return user?.UserId ?? string.Empty;
		}

		/// <summary> Registers an application user. </summary>
		/// <param name="input">The input for registering an application user</param>
		/// <example>
		/// <!-- Request example -->
		/// await RegisterApplicationUserAsync(new RegisterApplicationUserInput { UserId = "123", ApplicationId = "456" });
		/// <!-- Response example -->
		/// {
		///		Success: true,
		///		ErrorMessage: null,
		///		UserId: "123",
		///		ApplicationUserId: 1
		///	 }
		///	 <!-- Error response example -->
		///	 {
		///		Success: false,
		///		ErrorMessage: "User already exists",
		///		UserId: null,
		///		ApplicationUserId: 0
		///	 }
		///	 </example>
		public async Task<RegisterApplicationUserResult> RegisterApplicationUserAsync(RegisterApplicationUserInput input)
		{
			RegisterApplicationUserResult result = new RegisterApplicationUserResult();
			if(await UserExistsAsync(input.UserId, input.ApplicationId))
			{
				result.ErrorMessage = _t["User already exists"];
				result.Success = false;
				return result;
			}

			return result;
		}

		/// <summary>
		/// Updates an application user.
		/// </summary>
		/// <param name="input">UpdateApplicationUserInput</param>
		/// <returns>UpdateResult </returns>
		/// <example>
		/// <!-- Request example -->
		/// await UpdateApplicationUserAsync(new UpdateApplicationUserInput { Id = 1, UserDescription = "User description updated" });
		/// <!-- Response example -->
		/// {
		///   Success: true,
		///   UpdatedValues: { "UserDescription": "User description updated" },
		///   Errors: []
		/// }
		/// <!-- Error response example -->
		/// {
		///		Success: false,
		///		UpdateValues: {},
		///		Error: ["User not found"]
		///	 }
		///	 </example>
		public async Task<UpdateResult> UpdateApplicationUserAsync(UpdateApplicationUserInput input)
		{
			UpdateResult result = new UpdateResult();
			if(input == null)
			{
				result.Success = false;
				result.Errors.Add(_t["Input is null"]);
				return result;
			}
			if(input.Id == 0)
			{
				result.Success = false;
				result.Errors.Add(_t["Id is 0"]);
				return result;
			}
			ApplicationUser? user = await _context.ApplicationUsers.FindAsync(input.Id);
			if(user == null)
			{
				result.Success = false;
				result.Errors.Add(_t["User not found"]);
				return result;
			}
			if(input.UserDescription != user.UserDescription)
			{
				user.UserDescription = input.UserDescription;
				_ = await _context.SaveChangesAsync();
				result.UpdatedValues.Add("UserDescription", input.UserDescription ?? string.Empty);
				return result;
			}

			return result;
		}

		// Private functions
		private async Task<bool> UserExistsAsync(string userId, string applicationId)
		{
			return await _context.ApplicationUsers.AnyAsync(u => u.UserId == userId && u.ApplicationId == applicationId);
		}
	}
}