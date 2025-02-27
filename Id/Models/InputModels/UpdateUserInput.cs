using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for updating a user.
	/// </summary>
	public class UpdateUserInput : IUserInput
	{
		/// <summary>
		/// Gets or sets the user ID.
		/// </summary>
		[Required]
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user email.
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// Gets or sets the user password.
		/// </summary>
		public string? FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user password confirmation.
		/// </summary>
		public string? LastName { get; set; }

		/// <summary>
		/// Gets or sets Publicly visible name
		/// </summary>
		public string? DisplayedName { get; set; }

		/// <summary>
		/// Gets or sets Birthdate
		/// </summary>
		public DateTime? Birthdate { get; set; }

		/// <summary>
		/// Gets or sets Profile picture
		/// </summary>
		public IFormFile? ProfilePicture { get; set; }
	}
}