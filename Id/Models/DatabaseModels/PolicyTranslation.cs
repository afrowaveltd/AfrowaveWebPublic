namespace Id.Models.DatabaseModels
{
	public class PolicyTranslation
	{
		public int Id { get; set; }
		public int PolicyId { get; set; }
		public int LanguageId { get; set; }
		public string Content { get; set; } = string.Empty;
		public string UnapprovedContent { get; set; } = string.Empty;
		public string OldContent { get; set; } = string.Empty;
		public TranslationStatus Status { get; set; } = TranslationStatus.Default;
		public ApplicationPolicy Policy { get; set; } = null!;
		public Language Language { get; set; } = null!;
	}
}