namespace Id.Models.DatabaseModels
{
	public class ApplicationPolicy
	{
		public int Id { get; set; }
		public string ApplicationId { get; set; } = string.Empty;
		public PolicyType PolicyType { get; set; }
		public string OriginalLanguage { get; set; } = string.Empty;
		public Application Application { get; set; } = null!;
		public List<PolicyTranslation> Translations { get; set; } = new List<PolicyTranslation>();
	}
}