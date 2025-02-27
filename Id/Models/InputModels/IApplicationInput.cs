namespace Id.Models.InputModels
{
	/// <summary>
	/// Input model for creating an application.
	/// </summary>
	public interface IApplicationInput
	{
		/// <summary>
		/// Gets or sets the owner ID.
		/// </summary>
		string OwnerId { get; set; }

		/// <summary>
		/// Gets or sets the brand ID.
		/// </summary>
		int BrandId { get; set; }

		/// <summary>
		/// Gets or sets the application name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the application description.
		/// </summary>
		string? Description { get; set; }

		/// <summary>
		/// Gets or sets the application email.
		/// </summary>
		string? Email { get; set; }

		/// <summary>
		/// Gets or sets the application website.
		/// </summary>
		string? Website { get; set; }

		/// <summary>
		/// Gets or sets the application redirect URI.
		/// </summary>
		string? RedirectUri { get; set; }

		/// <summary>
		/// Gets or sets the application icon.
		/// </summary>
		IFormFile? Icon { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept terms and conditions.
		/// </summary>
		bool RequireTerms { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept privacy policy.
		/// </summary>
		bool RequirePrivacyPolicy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the application requires user to accept cookie policy.
		/// </summary>
		bool RequireCookiePolicy { get; set; }

		/// <summary>
		/// Gets or sets the URL to the privacy policy.
		/// </summary>
		string? PrivacyUrl { get; set; }

		/// <summary>
		/// Gets or sets the URL to the terms and conditions.
		/// </summary>
		string? TermsUrl { get; set; }

		/// <summary>
		/// Gets or sets the URL to the cookie policy.
		/// </summary>
		string? CookiesUrl { get; set; }
	}
}