using System.ComponentModel.DataAnnotations;

namespace Id.Models.InputModels
{
	public class UpdateApplicationInput : IApplicationInput
	{
		[Required]
		public string ApplicationId { get; set; } = string.Empty;

		[Required]
		public string OwnerId { get; set; } = string.Empty;

		[Required]
		public int BrandId { get; set; } = 0;

		[Required]
		public string Name { get; set; } = string.Empty;

		public string? Description { get; set; }
		public string? Email { get; set; }
		public string? Website { get; set; }
		public string? RedirectUri { get; set; }
		public IFormFile? Icon { get; set; }
		public bool RequireTerms { get; set; } = false;
		public bool RequirePrivacyPolicy { get; set; } = false;
		public bool RequireCookiePolicy { get; set; } = false;
		public string? PrivacyUrl { get; set; }
		public string? TermsUrl { get; set; }
		public string? CookiesUrl { get; set; }
	}
}