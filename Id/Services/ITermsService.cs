
namespace Id.Services
{
   public interface ITermsService
   {
		Task<string> GetCookiesHTMLAsync(string language);
		Task<string> GetTermsHtmlAsync(string language);
   }
}