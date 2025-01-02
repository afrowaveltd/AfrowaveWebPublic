namespace Id.Pages.Helps
{
   public class Install_BrandDescriptionModel(IStringLocalizer<Install_BrandDescriptionModel> _t) : PageModel
   {
      private readonly IStringLocalizer<Install_BrandDescriptionModel> t = _t;

      public string Title => t["Brand description"];
      public List<string> Lines = [];

      public void OnGet()
      {
         Lines.Add(t["The Brand description is a short text that describes your company."]);
         Lines.Add(t["The description is not required, but it is recommended to fill it in."]);
      }
   }
}