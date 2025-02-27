using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for updating an application.
	/// </summary>
	public class UpdateApplicationInput : IApplicationInput
	{
		/// <summary>
		/// Gets or sets the application ID.
		/// </summary>
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

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
		/// Gets or sets a value indicating whether the application requires user to accept privacy policy.
		/// </summary>
		public bool RequirePrivacyPolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept cookie policy.
		/// </summary>
		public bool RequireCookiePolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets the application privacy URL.
		/// </summary>
		public string? PrivacyUrl { get; set; }

		/// <summary>
		/// Gets or sets the application terms URL.
		/// </summary>
		public string? TermsUrl { get; set; }

		/// <summary>
		/// Gets or sets the application cookies URL.
		/// </summary>
		public string? CookiesUrl { get; set; }
	}
}