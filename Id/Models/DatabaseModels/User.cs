using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a user entity with its details.
	/// </summary>
	public class User
	{
		/// <summary>
		/// Gets or sets the unique identifier of the user.
		/// </summary>
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		/// <summary>
		/// Gets or sets the email address of the user.
		/// </summary>
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the phone number of the user.
		/// </summary>
		public string? PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the display name of the user.
		/// </summary>
		[Required]
		public string DisplayName { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the first name of the user.
		/// </summary>
		[Required]
		public string Firstname { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the last name of the user.
		/// </summary>
		[Required]
		public string Lastname { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the middle name of the user.
		/// </summary>
		public string? MiddleName { get; set; }

		/// <summary>
		/// Gets or sets the profile picture of the user.
		/// </summary>
		public string? ProfilePicture { get; set; }

		/// <summary>
		/// Gets or sets the password of the user.
		/// </summary>
		[Required]
		[JsonIgnore]
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the birth date of the user.
		/// </summary>
		public DateOnly? BirthDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

		/// <summary>
		/// Gets or sets the Gender of the user from enum
		/// </summary>
		public Gender Gender { get; set; } = Gender.Other;

		/// <summary>
		/// Gets or sets the mark if the user is owner of the system
		/// </summary>
		public bool IsOwner { get; set; } = false;

		/// <summary>
		/// Gets or sets the OTP token of the user.
		/// </summary>
		public string? OTPToken { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the email address is confirmed.
		/// </summary>
		public bool? EmailConfirmed { get; set; } = false;

		/// <summary>
		/// Gets or sets the token for resetting the password.
		/// </summary>
		public string? PasswordResetToken { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the date and time the password reset token expires.
		/// </summary>
		public DateTime? PasswordResetTokenExpiration { get; set; }

		/// <summary>
		/// Gets or sets the token for the OTP.
		/// </summary>
		public DateTime? OTPTokenExpiration { get; set; }

		/// <summary>
		/// Gets or sets the number of failed access attempts.
		/// </summary>
		public int AccessFailedCount { get; set; } = 0;

		/// <summary>
		/// Gets or sets the list of applications owned by the user.
		/// </summary>
		public List<Application> OwnedApplications { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of addresses of the user.
		/// </summary>
		public List<UserAddress> UserAddresses { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of refresh tokens of the user.
		/// </summary>
		public List<RefreshToken> RefreshTokens { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of applications the user has access to.
		/// </summary>
		public List<ApplicationUser> ApplicationUsers { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of brands the user has access to.
		/// </summary>
		public List<Brand> Brands { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of suspended applications.
		/// </summary>
		public List<SuspendedApplication> SuspendedApplications { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of suspended users.
		/// </summary>
		public List<SuspendedUser> Suspenders { get; set; } = [];
	}
}