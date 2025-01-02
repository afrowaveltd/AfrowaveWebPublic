
namespace Id.Services
{
   public interface ITermsService
   {
      Task<string> GetTermsHtmlAsync(string language);
   }
}