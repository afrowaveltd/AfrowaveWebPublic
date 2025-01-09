using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Services
{
	public interface ISelectOptionsServices
	{
		Task<List<SelectListItem>> GetBinaryOptionsAsync(bool selected = true);
		Task<string> GetDirectionAsync(string code);

		Task<List<SelectListItem>> GetLanguagesOptionsAsync(string selected);

		Task<List<SelectListItem>> GetSecureSocketOptionsAsync(SecureSocketOptions selected = SecureSocketOptions.Auto);

		Task<List<SelectListItem>> GetThemesAsync(string selected, string? userId);
	}
}