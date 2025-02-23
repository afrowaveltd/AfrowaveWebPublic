namespace Id.Pages.Helps
{
    public class Install_EmailModel(IStringLocalizer<Install_EmailModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_EmailModel> t = _t;

		public string Title1 { get; set; } = "";
        public string Title2 { get; set; } = "";
        public List<string> Lines = new List<string>();

        public IActionResult OnGet()
        {
            Title2 = t["Email address"];
            Lines.Add(t["The email address is used to send notifications and to recover your password."]);
            Lines.Add(t["It must be a valid email address."]);
            return Page();
        }
    }
}