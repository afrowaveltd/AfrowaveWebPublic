namespace Id.ConsoleToolkit.Models.SettingsModels
{
	public class ApplicationSettings
	{
		public Translator Translator { get; set; } = new();
		public string DefaultLanguage { get; set; } = "en";
		public string ApplicationId { get; set; } = string.Empty;
		public string SecretKey { get; set; } = string.Empty;
		public bool ApplicationConfigured { get; set; } = false;
	}

	public class Translator
	{
		public string Host { get; set; } = "https://translate.afrowave.ltd";
		public int Port { get; set; } = 443;
	}
}