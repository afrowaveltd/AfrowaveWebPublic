using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public interface IUsersManager
	{
		Task<string> GetBigImagePath(string userId);
		Task<string> GetIconPath(string userId);
		Task<string> GetMediumImagePath(string userId);
		Task<string> GetOriginalImagePath(string userId);
		Task<bool> IsEmailFreeAsync(string email);
		Task<RegisterUserResult> RegisterUserAsync(RegisterUserInput input);
		Task<UpdateResult> UpdateUserAsync(UpdateUserInput input);
	}
}