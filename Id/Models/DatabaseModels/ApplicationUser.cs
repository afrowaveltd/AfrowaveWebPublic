namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents an application user entity with its settings, roles, and suspension.
	/// </summary>
	public class ApplicationUser
	{
		/// <summary>
		/// Gets or sets the unique identifier associated with the current entity.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the application.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		public string UserId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the user description.
		/// </summary>
		public string? UserDescription { get; set; }

		/// <summary>
		/// Gets or sets the user's agreement to the terms and conditions.
		/// </summary>
		public bool AgreedToTerms { get; set; } = false;

		/// <summary>
		/// Gets or sets the user's agreement to share details with the application.
		/// </summary>
		public bool AgreedSharingUserDetails { get; set; } = false;

		/// <summary>
		/// Gets or sets the user's agreement for the application to use cookies.
		/// </summary>
		public bool AgreedToCookies { get; set; } = false;

		/// <summary>
		/// Gets or sets the date and time the entity was created.
		/// </summary>
		public DateTime Created { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the date and time until when the user is locked.
		/// </summary>
		public DateTime? LockedUntil { get; set; }

		/// <summary>
		/// Gets or sets the reason for locking the user.
		/// </summary>
		public string LockedReason { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the user is locked.
		/// </summary>
		public bool Locked { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the entity is deleted.
		/// </summary>
		public bool Deleted { get; set; } = false;

		// Privacy settings
		/// <summary>
		/// Gets or sets a value indicating whether the email address is visible to the application.
		/// </summary>
		public bool ShowEmail { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the phone number is visible to the application.
		/// </summary>
		public bool ShowPhone { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the address is visible to the application.
		/// </summary>
		public bool ShowAddress { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the name is visible to the application.
		/// </summary>
		public bool ShowName { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the profile picture is visible to the application.
		/// </summary>
		public bool ShowProfilePicture { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether the birthday is visible to the application.
		/// </summary>
		public bool ShowBirthday { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the gender is visible to the application.
		/// </summary>
		public bool ShowGender { get; set; } = false;

		/// <summary>
		/// Gets or sets the application associated with the entity.
		/// </summary>
		public Application? Application { get; set; }

		/// <summary>
		/// Gets or sets the user associated with the entity.
		/// </summary>
		public User? User { get; set; }

		/// <summary>
		/// Gets or sets the user roles associated with the entity.
		/// </summary>
		public List<UserRole> UserRoles { get; set; } = [];

		/// <summary>
		/// Gets or sets the suspended users associated with the entity.
		/// </summary>
		public List<SuspendedUser> SuspendedUsers { get; set; } = [];
	}
}