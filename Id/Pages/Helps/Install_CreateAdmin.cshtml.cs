namespace Id.Pages.Helps
{
    public class Install_CreateAdminModel : PageModel
    {
        private readonly IStringLocalizer<Install_CreateAdminModel> t;
        public string Title2 { get; set; } = "";
        public List<string> Lines = new List<string>();

        public Install_CreateAdminModel(IStringLocalizer<Install_CreateAdminModel> _t)
        {
            t = _t;
        }

        public IActionResult OnGet()
        {
            Title2 = t["Create administrator's account"];

            Lines.Add(t["To create an administrator's account, fill the form properly and click on the 'Next' button"]);

            return Page();
        }
    }
}