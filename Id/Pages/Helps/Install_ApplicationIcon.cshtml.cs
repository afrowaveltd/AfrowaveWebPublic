namespace Id.Pages.Helps
{
   public class Install_ApplicationIconModel(IStringLocalizer<Install_ApplicationIconModel> _t) : PageModel
   {
      private readonly IStringLocalizer<Install_ApplicationIconModel> t = _t;

      public string Title = string.Empty;
      public List<string> Lines = new();

      public void OnGet()
      {
         Title = t["Application Icon"];
         Lines.Add(t["The application icon is a small image that represents the application."]);
         Lines.Add(t["This icon will be displayed in the application list."]);
         Lines.Add(t["The icon is not required, but it is recommended to upload it."]);
         Lines.Add(t["The icon should be square and have a size of 150x150 pixels."]);
         Lines.Add(t["The icon should be in PNG or JPG format."]);
      }
   }
}