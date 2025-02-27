using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Account
{
	/// <summary>
	/// Represents the navigation pages for the account section.
	/// </summary>
	public class AccountNavPages
	{
		/// <summary>
		/// Gets the index page.
		/// </summary>
		public static string Index => "./Index";

		/// <summary>
		/// Gets the user registration page.
		/// </summary>
		public static string Register => "./Register";

		/// <summary>
		/// Gets the user login page.
		/// </summary>
		public static string Login => "./Login";

		/// <summary>
		/// Gets the user logout page.
		/// </summary>
		public static string Logout => "./Login";

		/// <summary>
		/// Gets the user profile page.
		/// </summary>
		public static string Profile => "./Profile";

		/// <summary>
		/// Gets the privacy settings page.
		/// </summary>
		public static string PrivacySettings => "./Privacy";

		/// <summary>
		/// Gets the change email page.
		/// </summary>
		public static string ChangeEmail => "./Email";

		/// <summary>
		/// Gets the change password page.
		/// </summary>
		public static string ChangePassword => "./Password";

		// public static string TwoFactorAuthentication => "#";

		/// <summary>
		/// Gets the personal data page.
		/// </summary>
		public static string PersonalData => "./PersonalData";

		/// <summary>
		/// Gets the delete account page.
		/// </summary>
		public static string DeleteAccount => "./DeleteAccount";

		/// <summary>
		/// Gets the index page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>Index webpage</returns>
		public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

		/// <summary>
		/// Gets the register page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>Registration webpage</returns>
		public static string RegisterNavClass(ViewContext viewContext) => PageNavClass(viewContext, Register);

		/// <summary>
		/// Gets the login page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>Login webpage</returns>
		public static string LoginNavClass(ViewContext viewContext) => PageNavClass(viewContext, Login);

		/// <summary>
		/// Gets the logout page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>Logout webpage</returns>
		public static string LogoutNavClass(ViewContext viewContext) => PageNavClass(viewContext, Logout);

		/// <summary>
		/// Gets the profile page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>User profile webpage</returns>
		public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

		/// <summary>
		/// Gets the privacy settings page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>Privacy settings webpage</returns>
		public static string PrivacySettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, PrivacySettings);

		/// <summary>
		/// Gets the change email page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>The email changing webpage</returns>
		public static string ChangeEmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangeEmail);

		/// <summary>
		/// Gets the change password page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>The password changing webpage</returns>
		public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

		/// <summary>
		/// Gets the personal data page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>The personal data webpage</returns>
		public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

		/// <summary>
		/// Gets the delete account page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <returns>Deleting account webpage</returns>
		public static string DeleteAccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeleteAccount);

		/// <summary>
		/// Gets the page class.
		/// </summary>
		/// <param name="viewContext">Decides what will be shown in the main frame</param>
		/// <param name="page">Selected webpage</param>
		/// <returns>Selected webpage</returns>
		public static string PageNavClass(ViewContext viewContext, string page)
		{
			string? activePage = viewContext.ViewData["ActivePage"] as string
				 ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
			return string.Equals(activePage, page, System.StringComparison.OrdinalIgnoreCase) ? "active" : "";
		}
	}
}