using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public class ApplicationUsersManager(IStringLocalizer<ApplicationUsersManager> t,
		ApplicationDbContext context)
	{
		// Initialization

		private readonly ApplicationDbContext _context = context;
		private readonly IStringLocalizer<ApplicationUsersManager> _t = t;

		// Public functions
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
			_context.ApplicationUsers.Remove(user);
			await _context.SaveChangesAsync();
			result.Success = true;
			return result;
		}

		public async Task<string> GetApplicationUserDescriptionByUserIdAsync(string userId, string applicationId)
		{
			ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.ApplicationId == applicationId);
			return user?.UserDescription ?? string.Empty;
		}

		public async Task<RegisterApplicationUserResult> RegisterApplicationUserAsync(RegisterApplicationUserInput input)
		{
			var result = new RegisterApplicationUserResult();
			if(await UserExistsAsync(input.UserId, input.ApplicationId))
			{
				result.ErrorMessage = _t["User already exists"];
				result.Success = false;
				return result;
			}

			return result;
		}

		public async Task<UpdateResult> UpdateApplicationUserAsync(UpdateApplicationUserInput input)
		{
			var result = new UpdateResult();
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
				await _context.SaveChangesAsync();
				result.UpdatedValues.Add("UserDescription", input.UserDescription);
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