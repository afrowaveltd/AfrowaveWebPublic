namespace Id.Models.DatabaseModels
{
	public class ApplicationUser
	{
		public int Id { get; set; }
		public string ApplicationId { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
		public string? UserDescription { get; set; }
		public bool AgreedToTerms { get; set; } = false;
		public bool AgreedSharingUserDetails { get; set; } = false;
		public bool AgreedToCookies { get; set; } = false;
		public DateTime Created { get; set; } = DateTime.Now;
		public DateTime? LockedUntil { get; set; }
		public string LockedReason { get; set; } = string.Empty;
		public bool Locked { get; set; } = false;
		public bool Deleted { get; set; } = false;

		// Privacy settings
		public bool ShowEmail { get; set; } = false;

		public bool ShowPhone { get; set; } = false;
		public bool ShowAddress { get; set; } = false;
		public bool ShowName { get; set; } = false;
		public bool ShowProfilePicture { get; set; } = true;
		public bool ShowBirthday { get; set; } = false;
		public bool ShowGender { get; set; } = false;

		public Application? Application { get; set; }
		public User? User { get; set; }
		public List<UserRole> UserRoles { get; set; } = [];
	}
}