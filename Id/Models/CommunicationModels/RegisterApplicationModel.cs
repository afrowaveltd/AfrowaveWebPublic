using System.ComponentModel.DataAnnotations;

namespace Id.Models.CommunicationModels
{
	public class RegisterApplicationModel
	{
		[Required]
		public string OwnerId { get; set; } = string.Empty;

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
		public bool AllowRememberConsent { get; set; } = true;
	}
}