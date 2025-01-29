namespace Id.Models.CommunicationModels
{
	public class ApplicationPublicInfo
	{
		public string ApplicationId { get; set; } = string.Empty;
		public string ApplicationName { get; set; } = string.Empty;
		public string ApplicationDescription { get; set; } = string.Empty;
		public string ApplicationWebsite { get; set; } = string.Empty;
		public string ApplicationLogoUrl { get; set; } = string.Empty;
		public string ApplicationPrivacyPolicyUrl { get; set; } = string.Empty;
		public string ApplicationTermsUrl { get; set; } = string.Empty;
		public string ApplicationCookiePolicyUrl { get; set; } = string.Empty;
		public bool RequireTerms { get; set; } = false;
		public bool RequirePrivacyPolicy { get; set; } = false;
		public bool RequireCookiePolicy { get; set; } = false;
		public string BrandName { get; set; } = string.Empty;
	}
}