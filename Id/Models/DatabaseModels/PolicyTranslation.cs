namespace Id.Models.DatabaseModels
{
	/// <summary>
	/// Represents a policy translation entity with its details.
	/// </summary>
	public class PolicyTranslation
	{
		/// <summary>
		/// Gets or sets the unique identifier of the policy translation.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the policy.
		/// </summary>
		public int PolicyId { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the language.
		/// </summary>
		public int LanguageId { get; set; }

		/// <summary>
		/// Gets or sets the content of the policy translation.
		/// </summary>
		public string Content { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the unapproved content of the policy translation.
		/// </summary>
		public string UnapprovedContent { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the old content of the policy translation.
		/// </summary>
		public string OldContent { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the status of the policy translation.
		/// </summary>
		public TranslationStatus Status { get; set; } = TranslationStatus.Default;

		/// <summary>
		/// Gets or sets the policy entity associated with the policy translation.
		/// </summary>
		public ApplicationPolicy Policy { get; set; } = null!;

		/// <summary>
		/// Gets or sets the language entity associated with the policy translation.
		/// </summary>
		public Language Language { get; set; } = null!;
	}
}