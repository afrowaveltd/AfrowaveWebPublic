using SharedTools.Services;

namespace Id.Services
{
	public class UiTranslatorService(ILogger<UiTranslatorService> logger, ITranslatorService translator) : IUiTranslatorService
	{
		private readonly ILogger<UiTranslatorService> _logger = logger;
		private readonly ITranslatorService _translator = translator;

		public async Task RunTranslationsAsync()
		{
			_logger.LogInformation("UiTranslatorService is starting.");
			/*
			 * 1. Load UI translator settings from appsettings.json
			 * 2. Load the default translations file from the file system
			 * 3. Check if old translations are available
			 * 4. Compare the default translations with the old translations
			 * 5. Add missing or changed values to the gueue for translation
			 * 6. In each language check if there are redundand translations,
			 * 7. If there are, remove them from the actual language file
			 * 8. Translate the values in the queue
			 * 9. Save the new translations to the file system
			 * 10. Do this with all supported languages, except the one which are in the settings list of exceptions.
			 * 11. End the cycle
			 */
			_logger.LogInformation("UiTranslatorService is done.");
		}
	}
}