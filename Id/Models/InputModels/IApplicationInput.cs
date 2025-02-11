namespace Id.Models.InputModels
{
	public interface IApplicationInput
	{
		string OwnerId { get; set; }
		int BrandId { get; set; }
		string Name { get; set; }
		string? Description { get; set; }
		string? Email { get; set; }
		string? Website { get; set; }
		string? RedirectUri { get; set; }
		IFormFile? Icon { get; set; }
		bool RequireTerms { get; set; }
		bool RequirePrivacyPolicy { get; set; }
		bool RequireCookiePolicy { get; set; }
		string? PrivacyUrl { get; set; }
		string? TermsUrl { get; set; }
		string? CookiesUrl { get; set; }
	}
}