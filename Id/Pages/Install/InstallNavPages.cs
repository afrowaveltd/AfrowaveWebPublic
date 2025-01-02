using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Pages.Install
{
   public static class InstallNavPages
   {
      public static string Index => "#";
      public static string Application => "#";

      public static string Brand => "#";
      public static string ApplicationRoles => "#";
      public static string SmtpSettings => "#";
      public static string LoginRules => "#";
      public static string PasswordRules => "#";
      public static string Cookie => "#";
      public static string JwtSettings => "#";

      public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

      public static string ApplicationNavClass(ViewContext viewContext) => PageNavClass(viewContext, Application);

      public static string BrandNavClass(ViewContext viewContext) => PageNavClass(viewContext, Brand);

      public static string RolesNavClass(ViewContext viewContext) => PageNavClass(viewContext, ApplicationRoles);

      public static string SmtpNavClass(ViewContext viewContext) => PageNavClass(viewContext, SmtpSettings);

      public static string LoginNavClass(ViewContext viewContext) => PageNavClass(viewContext, LoginRules);

      public static string PasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, PasswordRules);

      public static string CookieNavClass(ViewContext viewContext) => PageNavClass(viewContext, Cookie);

      public static string JwtNavClass(ViewContext viewContext) => PageNavClass(viewContext, JwtSettings);

      public static string PageNavClass(ViewContext viewContext, string page)
      {
         string? activePage = viewContext.ViewData["ActivePage"] as string
             ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
         return string.Equals(activePage, page, System.StringComparison.OrdinalIgnoreCase) ? "active" : "";
      }
   }
}