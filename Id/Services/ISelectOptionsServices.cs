using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Services
{
	/// <summary>
	/// Service to handle select options.
	/// </summary>
	public interface ISelectOptionsServices
	{
		/// <summary>
		/// Get the binary options.
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <returns>List of the Items for SELECT element</returns>
		Task<List<SelectListItem>> GetBinaryOptionsAsync(bool selected = true);

		/// <summary>
		/// Get the direction options.
		/// </summary>
		/// <param name="code">the Language code</param>
		/// <returns>ltr or rtl string depending on the language direction of writing</returns>
		Task<string> GetDirectionAsync(string code);

		/// <summary>
		/// Get the HTTP headers options
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <returns>List of items for SELECT element</returns>
		Task<List<SelectListItem>> GetHttpHeadersAsync(List<string> selected);

		/// <summary>
		/// Get the HTTP methods options
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <returns>List of items for SELECT element</returns>
		Task<List<SelectListItem>> GetHttpMethodsAsync(List<string> selected);

		/// <summary>
		/// Get the languages options
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <returns>List of items for SELECT element</returns>
		Task<List<SelectListItem>> GetLanguagesOptionsAsync(string selected);

		/// <summary>
		/// Get the same site mode options
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <returns>List of items for SELECT element</returns>
		Task<List<SelectListItem>> GetSameSiteModeOptionsAsync(SameSiteMode selected = SameSiteMode.Lax);

		/// <summary>
		/// Get the secure socket options
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <returns>List of items for SELECT element</returns>
		Task<List<SelectListItem>> GetSecureSocketOptionsAsync(SecureSocketOptions selected = SecureSocketOptions.Auto);

		/// <summary>
		/// Get the themes options
		/// </summary>
		/// <param name="selected">Selected value</param>
		/// <param name="userId">User ID</param>
		/// <returns>List of themes for SELECT element</returns>
		Task<List<SelectListItem>> GetThemesAsync(string selected, string? userId);
	}
}