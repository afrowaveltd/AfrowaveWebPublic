namespace Id.Pages.Helps
{
   public class Install_BrandLogoModel(IStringLocalizer<Install_BrandLogoModel> _t) : PageModel
   {
      private readonly IStringLocalizer<Install_BrandLogoModel> t = _t;

      public string Title => t["Brand logo"];
      public List<string> Lines = [];

      public void OnGet()
      {
         Lines.Add(t["The Brand logo is a visual representation of your company."]);
         Lines.Add(t["It is used in the title of the application."]);
         Lines.Add(t["The logo is not required, but it is recommended to upload it."]);
         Lines.Add(t["The logo should be square and have a size of 150x150 pixels."]);
         Lines.Add(t["The logo should be in PNG or JPG format."]);
      }
   }
}