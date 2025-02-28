using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Provides methods to manage users.
	/// </summary>
	public interface IUsersManager
	{
		/// <summary>
		/// Gets relative path for the big profile image.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>relative path for the big profile image</returns>
		Task<string> GetBigImagePath(string userId);

		/// <summary>
		/// Gets the icon path for the user.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Relative path for the user icon</returns>
		Task<string> GetIconPath(string userId);

		/// <summary>
		/// Gets the medium image path for the user.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Relative path for the medium size profile picture</returns>
		Task<string> GetMediumImagePath(string userId);

		/// <summary>
		/// Gets the original image path for the user.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Get the relative path for the User profile picture in its original size</returns>
		Task<string> GetOriginalImagePath(string userId);

		/// <summary>
		/// Checks if the email is free.
		/// </summary>
		/// <param name="email">Email address to be tested</param>
		/// <returns>True if address is not yet registered</returns>
		Task<bool> IsEmailFreeAsync(string email);

		/// <summary>
		/// Registers a new user.
		/// </summary>
		/// <param name="input">RegisterUserInput</param>
		/// <returns>RegisterUserResult</returns>
		Task<RegisterUserResult> RegisterUserAsync(RegisterUserInput input);

		/// <summary>
		/// Updates the user.
		/// </summary>
		/// <param name="input">UpdateUserAsync</param>
		/// <returns>UpdateResult</returns>
		Task<UpdateResult> UpdateUserAsync(UpdateUserInput input);
	}
}