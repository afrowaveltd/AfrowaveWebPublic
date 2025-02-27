using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for registering an application user.
	/// </summary>
	public class RegisterApplicationUserInput
	{
		/// <summary>
		/// Gets or sets the user ID.
		/// </summary>
		[Required]
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user description.
		/// </summary>
		public string? UserDescription { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has agreed to the terms and conditions.
		/// </summary>
		public bool AgreedToTerms { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the user has agreed to the privacy policy.
		/// </summary>
		public bool AgreedSharingUserDetails { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the user has agreed to cookies.
		/// </summary>
		public bool AgreedToCookies { get; set; } = false;
	}
}