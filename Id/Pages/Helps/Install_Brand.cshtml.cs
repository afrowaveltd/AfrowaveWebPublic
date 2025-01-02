namespace Id.Pages.Helps
{
   public class Install_BrandModel(IStringLocalizer<Install_BrandModel> _t) : PageModel
   {
      public IStringLocalizer<Install_BrandModel> t = _t;
      public string Title { get; set; } = "";
      public List<string> Lines = new List<string>();

      public void OnGet()
      {
         Title = t["Brand"];
         Lines.Add(t["The brand is the name of the company or organization that owns the application"]);
         Lines.Add(t["On this page you can enter the name of the brand, a logo, a website, a description and an email address"]);
      }
   }
}