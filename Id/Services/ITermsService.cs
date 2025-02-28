namespace Id.Services
{
	/// <summary>
	/// Service to handle terms.
	/// </summary>
	public interface ITermsService
	{
		/// <summary>
		/// Get the cookies HTML.
		/// </summary>
		/// <param name="language">Language for the page</param>
		/// <returns>Webpage with Cookies informations</returns>
		Task<string> GetCookiesHTMLAsync(string language);

		/// <summary>
		/// Get the privacy policy HTML.
		/// </summary>
		/// <param name="language">Language for the page</param>
		/// <returns>Terms and conditions for the application</returns>
		Task<string> GetTermsHtmlAsync(string language);
	}
}