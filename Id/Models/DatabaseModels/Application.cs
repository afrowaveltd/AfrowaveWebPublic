namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents an application entity with its settings, ownership, and policies.
	/// </summary>
	public class Application
	{
		/// <summary>
		/// Gets or sets the unique identifier for the application.
		/// </summary>
		public string Id { get; set; } = Guid.NewGuid().ToString();

		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a brief description of the application.
		/// </summary>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the contact email for the application.
		/// </summary>
		public string? ApplicationEmail { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the application has a logo.
		/// </summary>
		public bool Logo { get; set; } = false;

		/// <summary>
		/// Gets or sets the identifier of the owner of the application.
		/// </summary>
		public string OwnerId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the brand identifier associated with the application.
		/// </summary>
		public int BrandId { get; set; }

		/// <summary>
		/// Gets or sets the date and time the application was published.
		/// </summary>
		public DateTime Published { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the website URL of the application.
		/// </summary>
		public string? ApplicationWebsite { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the privacy policy URL of the application.
		/// </summary>
		public string? ApplicationPrivacyPolicy { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the terms and conditions URL of the application.
		/// </summary>
		public string? ApplicationTermsAndConditions { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the cookies policy URL of the application.
		/// </summary>
		public string? ApplicationCookiesPolicy { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the redirect URI for authentication flows.
		/// </summary>
		public string? RedirectUri { get; set; }

		/// <summary>
		/// Gets or sets the post-logout redirect URI for authentication flows.
		/// </summary>
		public string? PostLogoutRedirectUri { get; set; }

		/// <summary>
		/// Gets or sets the client secret, which is ignored during JSON serialization.
		/// </summary>
		[JsonIgnore]
		public string? ClientSecret { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether terms must be accepted before use.
		/// </summary>
		public bool RequireTerms { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the privacy policy must be accepted before use.
		/// </summary>
		public bool RequirePrivacyPolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the cookie policy must be accepted before use.
		/// </summary>
		public bool RequireCookiePolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the application is enabled.
		/// </summary>
		public bool IsEnabled { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether the application is deleted.
		/// </summary>
		public bool IsDeleted { get; set; } = false;

		/// <summary>
		/// Gets or sets the owner of the application.
		/// </summary>
		public User? Owner { get; set; }

		/// <summary>
		/// Gets or sets the brand associated with the application.
		/// </summary>
		public Brand? Brand { get; set; }

		/// <summary>
		/// Gets or sets the SMTP settings for the application.
		/// </summary>
		public ApplicationSmtpSettings? SmtpSettings { get; set; }

		/// <summary>
		/// Gets or sets the roles associated with the application.
		/// </summary>
		public List<ApplicationRole> Roles { get; set; } = [];

		/// <summary>
		/// Gets or sets the policies associated with the application.
		/// </summary>
		public List<ApplicationPolicy> Policies { get; set; } = [];

		/// <summary>
		/// Gets or sets the users associated with the application.
		/// </summary>
		public List<ApplicationUser> Users { get; set; } = [];

		/// <summary>
		/// Gets or sets the list of suspended applications.
		/// </summary>
		public List<SuspendedApplication> SuspendedApplications { get; set; } = [];

		/// <summary>
		/// Gets or sets the suspended users associated with this application.
		/// </summary>
		public List<SuspendedUser> SuspendedUsers { get; set; } = [];
	}
}