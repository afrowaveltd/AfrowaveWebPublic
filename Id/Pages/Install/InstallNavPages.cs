using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
	/// <summary>
	/// The navigation class for Installation pages
	/// </summary>
	public static class InstallNavPages
	{
		/// <summary>
		/// The index page
		/// </summary>
		public static string Index => "#";

		/// <summary>
		/// The application page
		/// </summary>
		public static string Application => "#";

		/// <summary>
		/// The brand page
		/// </summary>
		public static string Brand => "#";

		/// <summary>
		/// The application roles page
		/// </summary>
		public static string ApplicationRoles => "#";

		/// <summary>
		/// The SMTP settings page
		/// </summary>
		public static string SmtpSettings => "#";

		/// <summary>
		/// The login rules page
		/// </summary>
		public static string LoginRules => "#";

		/// <summary>
		/// The password rules page
		/// </summary>
		public static string PasswordRules => "#";

		/// <summary>
		/// The cookie page
		/// </summary>
		public static string Cookie => "#";

		/// <summary>
		/// The JWT settings page
		/// </summary>
		public static string JwtSettings => "#";

		/// <summary>
		/// The CORS settings page
		/// </summary>
		public static string CorsSettings => "#";

		/// <summary>
		/// The installation result page
		/// </summary>
		public static string InstallationResult => "#";

		/// <summary>
		/// The index navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

		/// <summary>
		/// The application navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string ApplicationNavClass(ViewContext viewContext) => PageNavClass(viewContext, Application);

		/// <summary>
		/// The brand navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string BrandNavClass(ViewContext viewContext) => PageNavClass(viewContext, Brand);

		/// <summary>
		/// The application roles navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string RolesNavClass(ViewContext viewContext) => PageNavClass(viewContext, ApplicationRoles);

		/// <summary>
		/// The SMTP settings navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string SmtpNavClass(ViewContext viewContext) => PageNavClass(viewContext, SmtpSettings);

		/// <summary>
		/// The login rules navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string LoginNavClass(ViewContext viewContext) => PageNavClass(viewContext, LoginRules);

		/// <summary>
		/// The password rules navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string PasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, PasswordRules);

		/// <summary>
		/// The cookie navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string CookieNavClass(ViewContext viewContext) => PageNavClass(viewContext, Cookie);

		/// <summary>
		/// The JWT settings navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string JwtNavClass(ViewContext viewContext) => PageNavClass(viewContext, JwtSettings);

		/// <summary>
		///		The CORS settings navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string CorsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CorsSettings);

		/// <summary>
		/// The installation result navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <returns></returns>
		public static string ResultNavClass(ViewContext viewContext) => PageNavClass(viewContext, InstallationResult);

		/// <summary>
		/// The page navigation class
		/// </summary>
		/// <param name="viewContext"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public static string PageNavClass(ViewContext viewContext, string page)
		{
			string? activePage = viewContext.ViewData["ActivePage"] as string
				 ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
			return string.Equals(activePage, page, System.StringComparison.OrdinalIgnoreCase) ? "active" : "";
		}
	}
}