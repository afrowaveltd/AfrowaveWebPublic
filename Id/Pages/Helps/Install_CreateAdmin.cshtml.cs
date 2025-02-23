namespace Id.Pages.Helps
{
    public class Install_CreateAdminModel(IStringLocalizer<Install_CreateAdminModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_CreateAdminModel> t = _t;
        public string Title2 { get; set; } = "";
        public List<string> Lines = new List<string>();

		public IActionResult OnGet()
        {
            Title2 = t["Create administrator's account"];

            Lines.Add(t["To create an administrator's account, fill the form properly and click on the 'Next' button"]);

            return Page();
        }
    }
}