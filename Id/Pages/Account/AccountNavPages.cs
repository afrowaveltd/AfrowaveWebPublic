using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Account
{
	public class AccountNavPages
	{
		public static string Index => "./Index";
		public static string Register => "./Register";
		public static string Login => "./Login";
		public static string Logout => "./Login";
		public static string Profile => "./Profile";
		public static string PrivacySettings => "./Privacy";
		public static string ChangeEmail => "./Email";
		public static string ChangePassword => "./Password";

		// public static string TwoFactorAuthentication => "#";

		public static string PersonalData => "./PersonalData";
		public static string DeleteAccount => "./DeleteAccount";

		public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

		public static string RegisterNavClass(ViewContext viewContext) => PageNavClass(viewContext, Register);

		public static string LoginNavClass(ViewContext viewContext) => PageNavClass(viewContext, Login);

		public static string LogoutNavClass(ViewContext viewContext) => PageNavClass(viewContext, Logout);

		public static string ProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

		public static string PrivacySettingsNavClass(ViewContext viewContext) => PageNavClass(viewContext, PrivacySettings);

		public static string ChangeEmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangeEmail);

		public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

		public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

		public static string DeleteAccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeleteAccount);

		public static string PageNavClass(ViewContext viewContext, string page)
		{
			string? activePage = viewContext.ViewData["ActivePage"] as string
				 ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
			return string.Equals(activePage, page, System.StringComparison.OrdinalIgnoreCase) ? "active" : "";
		}
	}
}