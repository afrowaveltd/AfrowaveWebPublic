namespace Id.Models.SettingsModels
{
	/// <summary>
	/// Represents the settings for the UI Translator.
	/// </summary>
	public class UITranslator
	{
		/// <summary>
		/// Gets or sets the default language for the application.
		/// </summary>
		public string DefaultLanguage { get; set; } = "en";

		/// <summary>
		/// Gets or sets languages that will not be translated.
		/// </summary>
		public List<string> IgnoredLanguages { get; set; } = [];
	}
}