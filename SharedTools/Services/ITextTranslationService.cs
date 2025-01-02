using SharedTools.Models;

namespace SharedTools.Services
{
   public interface ITextTranslationService
   {
      Task<string> TranslateAndFormatAsync(string input, string sourceLanguage, string targetLanguage);
      Task<ApiResponse<List<string>>> TranslateFolder(string folderPath);
      Task<ApiResponse<List<string>>> TranslateFolder(string folderPath, string language);
   }
}