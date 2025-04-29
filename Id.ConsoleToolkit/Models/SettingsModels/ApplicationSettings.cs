namespace Id.ConsoleToolkit.Models.SettingsModels
{
	/// <summary>
	/// Represents the application settings for the console application.
	/// </summary>
	public class ApplicationSettings
	{
		/// <summary>
		/// Gets or sets the application name.
		/// </summary>
		public Translator Translator { get; set; } = new();

		/// <summary>
		/// Gets or sets the default language for the application.
		/// </summary>
		public string DefaultLanguage { get; set; } = "cs";

		/// <summary>
		/// Gets or sets the application ID for the application.
		/// </summary>
		public string ApplicationId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application secret key for the application.
		/// </summary>
		public string SecretKey { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the application name for the application.
		/// </summary>
		public bool ApplicationConfigured { get; set; } = false;
	}

	/// <summary>
	/// Represents the translator settings for the application.
	/// </summary>
	public class Translator
	{
		/// <summary>
		/// Gets or sets the host URL for the translator service.
		/// </summary>
		public string Host { get; set; } = "https://translate.afrowave.ltd";

		/// <summary>
		/// Gets or sets the port for the translator service.
		/// </summary>
		public int Port { get; set; } = 443;
	}
}