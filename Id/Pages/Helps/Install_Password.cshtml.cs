namespace Id.Pages.Helps
{
    public class Install_PasswordModel(IStringLocalizer<Install_PasswordModel> _t) : PageModel
    {
        private readonly IStringLocalizer<Install_PasswordModel> t = _t;

        public string Title { get; set; } = "";
        public List<string> Lines = new List<string>();

		public IActionResult OnGet()
        {
            Title = t["Set the password"];

            Lines.Add(t["This will be a password for the administrator's account."]);
            Lines.Add(t["Make sure it is strong enough."]);

            return Page();
        }
    }
}