namespace Id.Models.SettingsModels
{
	public class UITranslator
	{
		public string DefaultLanguage { get; set; } = "en";
		public List<string> IgnoredLanguages { get; set; } = [];
	}
}