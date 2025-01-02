namespace Id.Pages.Helps
{
   public class Install_BrandWebsiteModel(IStringLocalizer<Install_BrandWebsiteModel> _t) : PageModel
   {
      public IStringLocalizer<Install_BrandWebsiteModel> t = _t;

      public string Title { get; set; } = "";
      public List<string> Lines = new List<string>();

      public void OnGet()
      {
         Title = t["Brand website"];
         Lines.Add(t["The brand website is the URL of the website of the company or organization that owns the application"]);
         Lines.Add(t["On this page you can enter the URL of the brand website"]);
      }
   }
}