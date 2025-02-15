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

		// Private functions
		private async Task<bool> UserExistsAsync(string userId, string applicationId)
		{
			return await _context.ApplicationUsers.AnyAsync(u => u.UserId == userId && u.ApplicationId == applicationId);
		}
	}
}