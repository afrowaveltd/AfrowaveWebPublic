namespace Id.Services
{
	/// <summary>
	/// Service to handle translations.
	/// </summary>
	public interface IUiTranslatorService
	{
		/// <summary>
		/// Check if the old language exists.
		/// </summary>
		/// <returns>True if old translations are presented</returns>
		bool OldLangaugeExists();

		/// <summary>
		/// Run the translations.
		/// </summary>
		/// <returns></returns>
		Task RunTranslationsAsync();
	}
}