using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for creating an application.
	/// </summary>
	public class RegisterApplicationInput : IApplicationInput
	{
		/// <summary>
		/// Gets or sets the owner ID.
		/// </summary>
		[Required]
		public string OwnerId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the brand ID.
		/// </summary>
		[Required]
		public int BrandId { get; set; } = 0;

		/// <summary>
		/// Gets or sets the application name.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application description.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the application email.
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// Gets or sets the application website.
		/// </summary>
		public string? Website { get; set; }

		/// <summary>
		/// Gets or sets the application redirect URI.
		/// </summary>
		public string? RedirectUri { get; set; }

		/// <summary>
		/// Gets or sets the application icon.
		/// </summary>
		public IFormFile? Icon { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept terms and conditions.
		/// </summary>
		public bool RequireTerms { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the application is an authenticator app.
		/// </summary>
		public bool IsAuthenticator { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept privacy policy.
		/// </summary>
		public bool RequirePrivacyPolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept cookie policy.
		/// </summary>
		public bool RequireCookiePolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets the privacy policy URL.
		/// </summary>
		public string? PrivacyUrl { get; set; }

		/// <summary>
		/// Gets or sets the terms and conditions URL.
		/// </summary>
		public string? TermsUrl { get; set; }

		/// <summary>
		/// Gets or sets the cookie policy URL.
		/// </summary>
		public string? CookiesUrl { get; set; }
	}
}