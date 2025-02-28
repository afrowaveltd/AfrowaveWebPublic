namespace Id.Services
{
	/// <summary>
	/// Interface for managing cookies.
	/// </summary>
	public interface ICookieService
	{
		/// <summary>
		/// Get the cookie value by key.
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <returns>Cookie value</returns>
		public string GetCookie(string key);

		/// <summary>
		/// Get or create a cookie with a key, value, and optional expiration time.
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <param name="value">Cookie value</param>
		/// <param name="expireTime">Expiration time</param>
		/// <returns>value of the cookie</returns>
		public string GetOrCreateCookie(string key, string value, int expireTime = 0);

		/// <summary>
		/// Remove all cookies.
		/// </summary>
		public void RemoveAllCookies();

		/// <summary>
		/// Remove a cookie by key.
		/// </summary>
		/// <param name="key">Key of the cookie to be removed</param>
		public void RemoveCookie(string key);

		/// <summary>
		/// Set a cookie with a key, value, and optional expiration time.
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <param name="value">Cookie value</param>
		/// <param name="expireTime">Expiration key</param>
		public void SetCookie(string key, string value, int expireTime = 0);

		/// <summary>
		/// Set a cookie with a key, value, and optional expiration time.
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <param name="value">Cookie value</param>
		/// <param name="expireTime">Expiration time</param>
		public void SetHttpOnlyCookie(string key, string value, int expireTime = 0);
	}
}