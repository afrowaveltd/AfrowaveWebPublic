using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for registering a user.
	/// </summary>
	public class RegisterUserInput : IUserInput
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		public string? ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the user email.
		/// </summary>
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user password.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user password confirmation.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		public string PasswordConfirm { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user gender.
		/// </summary>
		public Gender Gender { get; set; } = Gender.Other;

		/// <summary>
		/// Gets or sets the user first name.
		/// </summary>
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user last name.
		/// </summary>
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user displayed name.
		/// </summary>
		public string DisplayedName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user birthdate.
		/// </summary>
		public DateTime? Birthdate { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the user profile picture.
		/// </summary>
		public IFormFile? ProfilePicture { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user accepts terms and conditions.
		/// </summary>
		[Required]
		public bool AcceptTerms { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the user accepts privacy policy.
		/// </summary>
		[Required]
		public bool AcceptPrivacyPolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the user accepts cookie policy.
		/// </summary>
		[Required]
		public bool AcceptCookiePolicy { get; set; } = false;
	}
}