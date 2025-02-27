namespace Id.Models.InputModels
{
	/// <summary>
	/// Interface for user input models.
	/// </summary>
	public interface IUserInput
	{
		/// <summary>
		/// Email of the user.
		/// </summary>
		string Email { get; set; }

		/// <summary>
		/// First name of the user.
		/// </summary>
		string FirstName { get; set; }

		/// <summary>
		/// Last name of the user.
		/// </summary>
		string LastName { get; set; }

		/// <summary>
		/// Displayed name of the user.
		/// </summary>
		string? DisplayedName { get; set; }

		/// <summary>
		/// Birthdate of the user.
		/// </summary>
		DateTime? Birthdate { get; set; }

		/// <summary>
		/// Profile picture of the user.
		/// </summary>
		IFormFile? ProfilePicture { get; set; }
	}
}