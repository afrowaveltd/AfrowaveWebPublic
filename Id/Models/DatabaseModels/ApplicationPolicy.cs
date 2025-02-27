namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a policy entity with its translations.
	/// </summary>
	public class ApplicationPolicy
	{
		/// <summary>
		/// Gets or sets the identifier of the policy.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the application.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the type of the policy.
		/// </summary>
		public PolicyType PolicyType { get; set; }

		/// <summary>
		/// Gets or sets the original language of the policy.
		/// </summary>
		public string OriginalLanguage { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application entity associated with the policy.
		/// </summary>
		public Application Application { get; set; } = null!;

		/// <summary>
		/// Gets or sets the translations of the policy.
		/// </summary>
		public List<PolicyTranslation> Translations { get; set; } = [];
	}
}