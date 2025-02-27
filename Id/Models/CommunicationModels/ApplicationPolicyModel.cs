namespace Id.Models.CommunicationModels
{
	/// <summary>
	/// Application policy model.
	/// </summary>
	public class ApplicationPolicyModel
	{
		/// <summary>
		/// Gets or sets the application identifier.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether agree terms.
		/// </summary>
		public bool AgreeTerms { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether agree privacy policy.
		/// </summary>
		public bool AgreePrivacyPolicy { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether agree cookie policy.
		/// </summary>
		public bool AgreeCookiePolicy { get; set; } = false;
	}
}