using SharedTools.Models;

namespace SharedTools.Services
{
    public interface ITranslatorService
    {
        Task<ApiResponse<TranslateResponse>> AutodetectSourceLanguageAndTranslateAsync(string text, string targetLanguage);

        Task<string[]> GetSupportedLanguagesAsync();

        Task<ApiResponse<string>> TranslateAsync(string text, string sourceLanguage, string targetLanguage);
    }
}