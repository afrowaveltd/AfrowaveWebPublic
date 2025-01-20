
namespace Id.Services
{
	public interface IUiTranslatorService
	{
		bool OldLangaugeExists();
		Task RunTranslationsAsync();
	}
}