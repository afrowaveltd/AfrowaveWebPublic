namespace Id.Models.CommunicationModels
{
	public class ApplicationPolicyModel
	{
		public string ApplicationId { get; set; } = string.Empty;
		public bool AgreeTerms { get; set; } = false;
		public bool AgreePrivacyPolicy { get; set; } = false;
		public bool AgreeCookiePolicy { get; set; } = false;
	}
}