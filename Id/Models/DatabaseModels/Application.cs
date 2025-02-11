namespace Id.Models.DatabaseModels
{
	public class Application
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string? ApplicationEmail { get; set; }
		public bool Logo { get; set; } = false;
		public string OwnerId { get; set; } = string.Empty;
		public int BrandId { get; set; }
		public DateTime Published { get; set; } = DateTime.UtcNow;
		public string? ApplicationWebsite { get; set; } = string.Empty;
		public string? ApplicationPrivacyPolicy { get; set; } = string.Empty;
		public string? ApplicationTermsAndConditions { get; set; } = string.Empty;
		public string? ApplicationCookiesPolicy { get; set; } = string.Empty;
		public string? RedirectUri { get; set; }
		public string? PostLogoutRedirectUri { get; set; }

		[JsonIgnore]
		public string? ClientSecret { get; set; }

		public bool RequireTerms { get; set; } = false;
		public bool RequirePrivacyPolicy { get; set; } = false;
		public bool RequireCookiePolicy { get; set; } = false;
		public bool IsEnabled { get; set; } = true;
		public bool IsDeleted { get; set; } = false;
		public bool Suspended { get; set; } = false;
		public string? SuspendedById { get; set; } = string.Empty;
		public User? Owner { get; set; }
		public User? SuspendedBy { get; set; }
		public string? ReasonForSuspension { get; set; }
		public Brand? Brand { get; set; }
		public ApplicationSmtpSettings? SmtpSettings { get; set; }
		public List<ApplicationRole> Roles { get; set; } = [];
		public List<ApplicationPolicy> Policies { get; set; } = [];
		public List<ApplicationUser> Users { get; set; } = [];
	}
}