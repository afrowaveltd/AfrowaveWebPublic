namespace Id.Pages.Helps
{
   public class Install_BrandEmailModel(IStringLocalizer<Install_BrandEmailModel> _t) : PageModel
   {
      private readonly IStringLocalizer<Install_BrandEmailModel> t = _t;

      public string Title => t["Brand email"];
      public List<string> Lines = [];

      public void OnGet()
      {
         Lines.Add(t["This should be a valid email address."]);
         Lines.Add(t["This email address will be used to send emails to users."]);
         Lines.Add(t["It will be public for users to see."]);
      }
   }
}